using System;
using Microsoft.Xna.Framework;

namespace Stroids.Game
{
    internal class Bullet : ScreenObject
    {
        private int _life;

        public Bullet()
            : base(new Point(0, 0))
        {
            _life = 0;
            InitPoints();
        }

        public bool Available()
        {
            return _life == 0;
        }

        public void Disable()
        {
            _life = 0;
        }

        public override void Draw(ScreenCanvas sc, int x, int y)
        {
            if (!Available())
            {
                base.Draw(sc, x, y, GetRandomFireColor());
            }
        }

        private void InitPoints()
        {
            AddPoint(new Point(0, -35));
            AddPoint(new Point(35, 0));
            AddPoint(new Point(0, 35));
            AddPoint(new Point(-35, 0));
        }

        public override bool Move()
        {
            if (!Available())
            {
                _life--;
            }
            return base.Move();
        }

        public void Shoot(Point locParent, float radParent, float velXParent, float velYParent)
        {
            _life = 60;
            CurrLoc = locParent;
            Radians = radParent;

            VelocityX = (int)(-100 * Math.Sin(Radians)) + velXParent;
            VelocityY = (int)(100 * Math.Cos(Radians)) + velYParent;
        }
    }
}