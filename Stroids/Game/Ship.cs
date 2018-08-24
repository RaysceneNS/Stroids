using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Stroids.Game
{
    internal class Ship : ScreenObject
    {
        private const float RotateSpeed = 200;
        private const float MaxVelocity = 83.3333333333333f;
        private const float DecayRate = 0.993333333333333f;
        private const int PointThrust1 = 3;
        private const int PointThrust2 = 4;
        private bool _thrustOn;
        private ShipState _state;

        public Ship()
            : base(new Point(5000, 3750))
        {
            _thrustOn = false;
            _state = ShipState.Alive;
            InitPoints();
        }

        public void DecayThrust()
        {
            _thrustOn = false;
            VelocityX = VelocityX * DecayRate;
            VelocityY = VelocityY * DecayRate;
        }

        public override void Draw(ScreenCanvas sc, int x, int y)
        {
            if (_state != ShipState.Alive)
                return;

            if (_thrustOn)
            {
                var a = PointsTransformed[PointThrust1];
                var b = PointsTransformed[PointThrust2];
                var num = RndGen.Next(200) + 100;
                var list = new List<Point>(3)
                {
                    a,
                    b,
                    new Point((a.X + b.X) / 2 + (int) (num * Math.Sin(Radians)), (a.Y + b.Y) / 2 + (int) (-num * Math.Cos(Radians)))
                };

                DrawPolyToScreen(list, sc, x, y, GetRandomFireColor());
            }

            base.Draw(sc, x, y);
        }

        public void Explode()
        {
            _state = ShipState.Exploding;
            VelocityY = 0;
            VelocityX = 0;
        }

        public bool Hyperspace()
        {
            CurrLoc.X = RndGen.Next(8000) + 1000;
            CurrLoc.Y = RndGen.Next(6000) + 750;
            return RndGen.Next(10) != 1;
        }

        private void InitPoints()
        {
            AddPoint(new Point(0, -200));
            AddPoint(new Point(50, 0));
            AddPoint(new Point(100, 200));
            AddPoint(new Point(30, 120));
            AddPoint(new Point(-30, 120));
            AddPoint(new Point(-100, 200));
            AddPoint(new Point(-50, 0));
        }

        public bool IsAlive()
        {
            return _state == ShipState.Alive;
        }

        public void RotateLeft()
        {
            Rotate(-RotateSpeed);
        }

        public void RotateRight()
        {
            Rotate(RotateSpeed);
        }

        public void Thrust()
        {
            _thrustOn = true;

            VelocityX += (float)-(1.5 * Math.Sin(Radians));
            if (VelocityX > MaxVelocity)
                VelocityX = MaxVelocity;
            if (VelocityX < -MaxVelocity)
                VelocityX = -MaxVelocity;

            VelocityY += (float)(1.5 * Math.Cos(Radians));
            if (VelocityY > MaxVelocity)
                VelocityY = MaxVelocity;
            if (VelocityY < -MaxVelocity)
                VelocityY = -MaxVelocity;
        }

        private enum ShipState
        {
            Alive,
            Exploding
        }
    }
}