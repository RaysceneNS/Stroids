﻿using System;
using System.Drawing;

namespace Stroids
{
    internal class Asteroid : ScreenObject
    {
        private const int SizeIncr = 220;
        private AsteroidSize _size;
        private double _rotateSpeed;

        public Asteroid(AsteroidSize size)
            : base(new Point(0, 0))
        {
            _size = size;
            CurrLoc.X = RndGen.Next(2) * 9999;
            CurrLoc.Y = RndGen.Next(7499);
            RandomVelocity();
            InitPoints();
        }

        public Asteroid(Asteroid copy)
            : base(copy.CurrLoc)
        {
            _size = copy._size;
            RandomVelocity();
            InitPoints();
        }

        public bool CheckPointInside(Point ptCheck)
        {
            return Math.Sqrt(Math.Pow(ptCheck.X - CurrLoc.X, 2) + Math.Pow(ptCheck.Y - CurrLoc.Y, 2)) <=
                   (int) _size * SizeIncr;
        }

        public override void Draw(ScreenCanvas sc, int x, int y)
        {
            if (_size != AsteroidSize.Dne)
                base.Draw(sc, x, y);
        }

        private void InitPoints()
        {
            Points.Clear();
            PointsTransformed.Clear();
            for (var i = 0; i < 9; i++)
            {
                var num = i * 40 * 0.0174532925199433;
                AddPoint(new Point((int) (Math.Sin(num) * -((int) _size * SizeIncr)),
                    (int) (Math.Cos(num) * ((int) _size * SizeIncr))));
            }
        }

        public override bool Move()
        {
            if (_size != AsteroidSize.Dne)
                Rotate(_rotateSpeed);
            return base.Move();
        }

        private void RandomVelocity()
        {
            _rotateSpeed = (double) (RndGen.Next(10000) - 5000) / 60;
            VelocityX = (RndGen.NextDouble() * 2000 - 1000) * ((3 - (int) _size + 1) * 1.05) / 60;
            VelocityY = (RndGen.NextDouble() * 2000 - 1000) * ((3 - (int) _size + 1) * 1.05) / 60;
        }

        public AsteroidSize ReduceSize()
        {
            if (_size != AsteroidSize.Dne)
            {
                _size -= (int) AsteroidSize.Small;
            }
            InitPoints();
            RandomVelocity();
            return _size;
        }
    }

    public enum AsteroidSize
    {
        Dne,
        Small,
        Medium,
        Large
    }
}