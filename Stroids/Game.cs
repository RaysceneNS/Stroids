using System.Drawing;

namespace Stroids
{
    public class Game
    {
        private const int PauseInterval = 60;
        private readonly Explosions _explosions;
        private readonly Bullet[] _shipBullets;
        private AsteroidBelt _asteroids;
        private int _iLevel;
        private bool _inProcess;
        private int _iPauseTimer;
        private bool _paused;
        private Ship _ship;

        public Game()
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
            _explosions = new Explosions();
            _paused = false;
            _iPauseTimer = PauseInterval;
        }

        private bool CheckPointInAsteroid(Point ptCheck, ref Score score)
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

        public void Draw(ScreenCanvas sc, int x, int y, ref Score score)
        {
            if (_paused)
            {
                if (_iPauseTimer > 30)
                    sc.AddText("PAUSE", Justify.Center, 2500, 200, 400, x, y);

                _iPauseTimer--;
                if (_iPauseTimer < 0)
                    _iPauseTimer = PauseInterval;
            }
            else
            {
                if (!_ship.IsAlive() && _explosions.Count() == 0)
                {
                    if (!score.HasReserveShips())
                    {
                        _inProcess = false;
                    }
                    else if (_asteroids.IsCenterSafe())
                    {
                        score.GetNewShip();
                        _ship = new Ship();
                    }
                }
                if (_explosions.Count() == 0 && _asteroids.Count() == 0)
                {
                    _iLevel++;
                    _asteroids = new AsteroidBelt(_iLevel);
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
                    if (!bullet.Available() && CheckPointInAsteroid(point, ref score))
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
                        if (!CheckPointInAsteroid(point, ref score))
                            continue;
                        ExplodeShip();
                        break;
                    }
                }
            }
            _ship.Draw(sc, x, y);

            foreach (var bullet in _shipBullets)
            {
                bullet.Draw(sc, x, y);
            }
            _asteroids.Draw(sc, x, y);
            _explosions.Draw(sc, x, y);
            score.Draw(sc, x, y);
        }

        private void ExplodeShip()
        {
            Sounds.Play(Sound.BangSmall);
            Sounds.Play(Sound.BangMedium);
            Sounds.Play(Sound.BangLarge);
            _ship.Explode();
            var currLoc = _ship.GetCurrLoc();
            foreach (var point in _ship.PointsTransformed)
            {
                _explosions.AddExplosion(new Point(point.X + currLoc.X, point.Y + currLoc.Y), 2);
            }
        }

        public void HyperspaceJump()
        {
            if (!_paused && _ship.IsAlive() && !_ship.Hyperspace())
                ExplodeShip();
        }

        public void Left()
        {
            if (!_paused && _ship.IsAlive())
                _ship.RotateLeft();
        }

        public void Pause()
        {
            _iPauseTimer = PauseInterval;
            _paused = !_paused;
        }

        public void Right()
        {
            if (!_paused && _ship.IsAlive())
                _ship.RotateRight();
        }

        public void Shoot()
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
                    bullet.Shoot(_ship.GetCurrLoc(), _ship.GetRadians(), _ship.GetVelocityX(), _ship.GetVelocityY());
                    Sounds.Play(Sound.Fire);
                    break;
                }
            }
        }

        public void Thrust(bool bThrustOn)
        {
            if (_paused || !_ship.IsAlive())
            {
                return;
            }

            _ship.DecayThrust();
            if (bThrustOn)
                _ship.Thrust();
        }
    }


    public enum Modes
    {
        PREP,
        TITLE,
        GAME,
        EXIT
    }
}