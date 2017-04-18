using System.Drawing;

namespace Stroids
{
    internal class Explosion : ScreenObject
    {
        private const int NumExpPoints = 22;
        private const int ExplosionLife = 30;
        private readonly Point[] _points;
        private readonly Point[] _pointsVelocity;
        private int _lifeLeft;

        public Explosion(Point ptExplosion, double timeFactor)
            : base(ptExplosion)
        {
            _lifeLeft = (int) (ExplosionLife * timeFactor);
            _points = new Point[NumExpPoints];
            _pointsVelocity = new Point[NumExpPoints];
            for (var i = 0; i < NumExpPoints; i++)
            {
                _points[i] = ptExplosion;
                _pointsVelocity[i] = new Point((int) ((double) (RndGen.Next(1200) - 600) / 60), (int) ((double) (RndGen.Next(1200) - 600) / 60));
            }
        }

        public override void Draw(ScreenCanvas sc, int x, int y)
        {
            for (var i = 0; i < NumExpPoints; i++)
            {
                var point = new Point((int) ((double) _points[i].X / 10000 * x), (int) ((double) _points[i].Y / 7500 * y));
                sc.AddLine(point, new Point(point.X + 1, point.Y + 1), GetRandomFireColor());
            }
        }
        
        public override bool Move()
        {
            if (_lifeLeft <= 0)
                return false;

            for (var i = 0; i < NumExpPoints; i++)
            {
                _points[i].X += _pointsVelocity[i].X;
                _points[i].Y += _pointsVelocity[i].Y;
                if (_points[i].X < 0)
                    _points[i].X = 9999;
                if (_points[i].X >= 10000)
                    _points[i].X = 0;
                if (_points[i].Y < 0)
                    _points[i].Y = 7499;
                if (_points[i].Y >= 7500)
                    _points[i].Y = 0;
            }
            _lifeLeft --;
            return true;
        }
    }
}