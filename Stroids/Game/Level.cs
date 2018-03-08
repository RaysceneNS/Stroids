using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Stroids.Game
{
    /// <summary>
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    internal class Level
    {
        private Ship _ship;
        private Explosions _explosions;
        private Bullet[] _shipBullets;
        private AsteroidBelt _asteroids;
        private int _iLevel;
        private bool _inProcess;
        private const int PauseInterval = 60;
        private int _iPauseTimer;
        private bool _paused;
        private readonly SoundEffectInstance _soundThrustInstance;
        private readonly Score _score;
        private readonly SoundEffect _extraShipSound;
        private readonly SoundEffect _soundBangLarge;
        private readonly SoundEffect _soundBangMedium;
        private readonly SoundEffect _soundBangSmall;
        private readonly SoundEffect _soundFire;
        private int _shotsFired;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider">The service provider that will be used to construct a ContentManager.</param>
        /// <param name="score"></param>
        public Level(IServiceProvider serviceProvider, Score score)
        {
            // Create a new content manager to load content used just by this level.
            var content = new ContentManager(serviceProvider, "Content");

            // Load sounds.
            var soundThrust = content.Load<SoundEffect>("Sounds/Thrust");
            _soundThrustInstance = soundThrust.CreateInstance();
            _extraShipSound = content.Load<SoundEffect>("Sounds/ExtraShip");
            _soundBangLarge = content.Load<SoundEffect>("Sounds/BangLarge");
            _soundBangMedium = content.Load<SoundEffect>("Sounds/BangMedium");
            _soundBangSmall = content.Load<SoundEffect>("Sounds/BangSmall");
            _soundFire = content.Load<SoundEffect>("Sounds/Fire");

            _score = score;
            _score.OnExtraShip += delegate { _extraShipSound.Play(); };
        }

        public Ship Ship
        {
            get { return _ship; }
        }

        public int ShotsFired
        {
            get { return _shotsFired; }
        }

        internal void StartGame()
        {
            _iLevel = 4;
            _inProcess = true;
            _ship = new Ship();
            _shipBullets = new Bullet[4];
            for (var i = 0; i < 4; i++)
            {
                _shipBullets[i] = new Bullet();
            }
            _asteroids = new AsteroidBelt(_iLevel);
            _asteroids.OnLargeExplosion += delegate { _soundBangLarge.Play(); };
            _asteroids.OnMediumExplosion += delegate { _soundBangMedium.Play(); };
            _asteroids.OnSmallExplosion += delegate { _soundBangSmall.Play(); };

            _explosions = new Explosions();
            _paused = false;
            _iPauseTimer = PauseInterval;
        }

        private void ExplodeShip()
        {
            _soundThrustInstance.Pause();
            _soundBangLarge.Play();
            _soundBangMedium.Play();

            _ship.Explode();
            var currLoc = _ship.GetCurrLoc();
            foreach (var point in _ship.PointsTransformed)
            {
                _explosions.AddExplosion(new Point(point.X + currLoc.X, point.Y + currLoc.Y), 2);
            }
        }

        private void HyperspaceJump()
        {
            if (!_paused && _ship.IsAlive() && !_ship.Hyperspace())
                ExplodeShip();
        }

        internal void Left()
        {
            if (!_paused && _ship.IsAlive())
                _ship.RotateLeft();
        }

        private void Pause()
        {
            _iPauseTimer = PauseInterval;
            _paused = !_paused;
        }

        internal void Right()
        {
            if (!_paused && _ship.IsAlive())
                _ship.RotateRight();
        }

        internal void Shoot()
        {
            if (_paused || !_ship.IsAlive())
            {
                return;
            }

            var num = 0;
            while (num < _shipBullets.Length)
            {
                var bullet = _shipBullets[num];
                if (!bullet.Available())
                {
                    num++;
                }
                else
                {
                    _shotsFired = ShotsFired + 1;
                    bullet.Shoot(_ship.GetCurrLoc(), _ship.GetRadians(), _ship.GetVelocityX(), _ship.GetVelocityY());
                    _soundFire.Play();
                    break;
                }
            }
        }

        private bool CheckPointInAsteroid(Point ptCheck, Score score)
        {
            var num = _asteroids.CheckPointCollisions(ptCheck);
            if (num <= 0)
                return false;
            score.AddScore(num);
            return true;
        }

        public bool Done()
        {
            return !_inProcess;
        }

        internal void Thrust(bool bThrustOn)
        {
            if (_paused || !_ship.IsAlive())
            {
                return;
            }

            if (bThrustOn)
            {
                _ship.Thrust();

                _soundThrustInstance.Play();
            }
            else
            {
                _ship.DecayThrust();
                _soundThrustInstance.Pause();
            }
        }

        public AsteroidsGame.GameState Update(KeyboardState state, KeyboardState previousState)
        {
            if (state.IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
            {
                return AsteroidsGame.GameState.Title;
            }

            if (state.IsKeyDown(Keys.Down) && !previousState.IsKeyDown(Keys.Down))
            {
                this.HyperspaceJump();
            }

            if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
            {
                this.Shoot();
            }

            if (state.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P))
            {
                this.Pause();
            }

            if (state.IsKeyDown(Keys.Left))
            {
                this.Left();
            }

            if (state.IsKeyDown(Keys.Right))
            {
                this.Right();
            }

            var upPressed = state.IsKeyDown(Keys.Up);
            this.Thrust(upPressed);

            //Draw(_screenCanvas, windowClientBounds.Width, windowClientBounds.Height, _score);
            if (_paused)
            {
                //if (_iPauseTimer > 30)
                //{
                //    sc.AddText("PAUSE", Justify.Center, 2500, 200, 400, width, height);
                //}

                _iPauseTimer--;
                if (_iPauseTimer < 0)
                {
                    _iPauseTimer = PauseInterval;
                }
            }
            else
            {
                if (!_ship.IsAlive() && _explosions.Count() == 0)
                {
                    if (!_score.HasReserveShips())
                    {
                        _inProcess = false;
                    }
                    else if (_asteroids.IsCenterSafe())
                    {
                        _score.UseNewShip();
                        _ship = new Ship();
                    }
                }
                if (_explosions.Count() == 0 && !_asteroids.Any())
                {
                    _iLevel++;
                    _asteroids.StartBelt(_iLevel, AsteroidSize.Large);
                }

                _ship.Move();
                foreach (var bullet in _shipBullets)
                {
                    bullet.Move();
                }
                _asteroids.Move();
                _explosions.Move();

                foreach (var bullet in _shipBullets)
                {
                    var point = bullet.GetCurrLoc();
                    if (!bullet.Available() && CheckPointInAsteroid(point, _score))
                    {
                        _explosions.AddExplosion(point);
                        bullet.Disable();
                    }
                }

                if (_ship.IsAlive())
                {
                    foreach (var shipPoint in _ship.PointsTransformed)
                    {
                        var currLoc = _ship.GetCurrLoc();
                        var point = new Point(shipPoint.X + currLoc.X, shipPoint.Y + currLoc.Y);
                        if (!CheckPointInAsteroid(point, _score))
                        {
                            continue;
                        }
                        ExplodeShip();
                        break;
                    }
                }
            }

            // return the state that we should move to, title or game
            return this.Done() ? AsteroidsGame.GameState.Title : AsteroidsGame.GameState.Game;
        }

        public AsteroidBelt AsteroidBelt
        {
            get { return _asteroids; }
        }

        internal void Draw(ScreenCanvas sc, int width, int height)
        {
            if (_paused)
            {
                if (_iPauseTimer > 30)
                {
                    sc.AddText("PAUSE", Justify.Center, 2500, 200, 400, width, height);
                }
            }
            _ship.Draw(sc, width, height);

            foreach (var bullet in _shipBullets)
            {
                bullet.Draw(sc, width, height);
            }
            _asteroids.Draw(sc, width, height);
            _explosions.Draw(sc, width, height);
            //score.Draw(sc, width, height);
        }

        public bool CanShoot()
        {
            foreach (var bullet in _shipBullets)
            {
                if (bullet.Available())
                    return true;
            }
            return false;
        }
    }
}
