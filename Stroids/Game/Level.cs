using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Stroids.Game
{
    /// <summary>
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    internal class Level
    {
        private Explosions _explosions;
        private Bullet[] _shipBullets;
        private int _iLevel;
        private bool _inProcess;
        private const int PauseInterval = 60;
        private int _iPauseTimer;
        private bool _paused;
        private readonly Score _score;
        private byte[] _asteroidBeltState;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        public Level(Score score)
        {
            _score = score;
        }

        public Ship Ship { get; private set; }

        public int ShotsFired { get; private set; }


        /// <summary>
        /// Start game with asteroids in same position as last play.
        /// </summary>
        internal void ReStartGame()
        {
            _iLevel = 4;
            _inProcess = true;
            Ship = new Ship();
            _shipBullets = new Bullet[4];
            for (var i = 0; i < 4; i++)
            {
                _shipBullets[i] = new Bullet();
            }
            AsteroidBelt = new AsteroidBelt(_iLevel);
            AsteroidBelt.LoadState(_asteroidBeltState);

            _explosions = new Explosions();
            _paused = false;
            _iPauseTimer = PauseInterval;
        }

        internal void StartGame()
        {
            _iLevel = 4;
            _inProcess = true;
            Ship = new Ship();
            _shipBullets = new Bullet[4];
            for (var i = 0; i < 4; i++)
            {
                _shipBullets[i] = new Bullet();
            }
            AsteroidBelt = new AsteroidBelt(_iLevel);
            _asteroidBeltState = AsteroidBelt.SaveState();

            _explosions = new Explosions();
            _paused = false;
            _iPauseTimer = PauseInterval;
        }

        private void ExplodeShip()
        {
            Ship.Explode();
            var currLoc = Ship.GetCurrentLocation();
            foreach (var point in Ship.PointsTransformed)
            {
                _explosions.AddExplosion(new Point(point.X + currLoc.X, point.Y + currLoc.Y), 2);
            }
        }

        private void HyperspaceJump()
        {
            if (!_paused && Ship.IsAlive() && !Ship.Hyperspace())
            {
                ExplodeShip();
            }
        }

        internal void Left()
        {
            if (!_paused && Ship.IsAlive())
            {
                Ship.RotateLeft();
            }
        }

        private void Pause()
        {
            _iPauseTimer = PauseInterval;
            _paused = !_paused;
        }

        internal void Right()
        {
            if (!_paused && Ship.IsAlive())
            {
                Ship.RotateRight();
            }
        }

        internal void Shoot()
        {
            if (_paused || !Ship.IsAlive())
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
                    ShotsFired = ShotsFired + 1;
                    bullet.Shoot(Ship.GetCurrentLocation(), Ship.GetRadians(), Ship.GetVelocityX(), Ship.GetVelocityY());
                    break;
                }
            }
        }

        private bool CheckPointInAsteroid(Point ptCheck, Score score)
        {
            var num = AsteroidBelt.CheckPointCollisions(ptCheck);
            if (num <= 0)
            {
                return false;
            }
            score.AddScore(num);
            return true;
        }

        public bool IsDone()
        {
            return !_inProcess;
        }

        internal void Thrust(bool bThrustOn)
        {
            if (_paused || !Ship.IsAlive())
            {
                return;
            }

            if (bThrustOn)
            {
                Ship.Thrust();
            }
            else
            {
                Ship.DecayThrust();
            }
        }

        public GameState Update(KeyboardState state, KeyboardState previousState)
        {
            if (state.IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
            {
                return GameState.Title;
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
                if (!Ship.IsAlive() && _explosions.Count() == 0)
                {
                    if (!_score.HasReserveShips())
                    {
                        _inProcess = false;
                    }
                    else if (AsteroidBelt.IsCenterSafe())
                    {
                        _score.UseNewShip();
                        Ship = new Ship();
                    }
                }
                if (_explosions.Count() == 0 && !AsteroidBelt.Any())
                {
                    _iLevel++;
                    AsteroidBelt.StartBelt(_iLevel, AsteroidSize.Large);
                }

                Ship.Move();
                foreach (var bullet in _shipBullets)
                {
                    bullet.Move();
                }
                AsteroidBelt.Move();
                _explosions.Move();

                foreach (var bullet in _shipBullets)
                {
                    var point = bullet.GetCurrentLocation();
                    if (!bullet.Available() && CheckPointInAsteroid(point, _score))
                    {
                        _explosions.AddExplosion(point);
                        bullet.Disable();
                    }
                }

                if (Ship.IsAlive())
                {
                    foreach (var shipPoint in Ship.PointsTransformed)
                    {
                        var currLoc = Ship.GetCurrentLocation();
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
            return this.IsDone() ? GameState.Title : GameState.Game;
        }

        public AsteroidBelt AsteroidBelt { get; private set; }

        internal void Draw(ScreenCanvas sc, int width, int height)
        {
            if (_paused)
            {
                if (_iPauseTimer > 30)
                {
                    sc.AddText("PAUSE", Justify.Center, 2500, 200, 400, width, height);
                }
            }
            Ship.Draw(sc, width, height);

            foreach (var bullet in _shipBullets)
            {
                bullet.Draw(sc, width, height);
            }
            AsteroidBelt.Draw(sc, width, height);
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
