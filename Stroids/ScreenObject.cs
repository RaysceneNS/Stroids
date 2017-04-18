using System;
using System.Collections.Generic;
using System.Drawing;

namespace Stroids
{
    internal abstract class ScreenObject
    {
        protected readonly List<Point> Points;
        public readonly List<Point> PointsTransformed;
        protected Point CurrLoc;
        protected double Radians;
        protected double VelocityX;
        protected double VelocityY;
        protected static readonly Random RndGen;

        static ScreenObject()
        {
            RndGen = new Random();
        }

        protected ScreenObject(Point location)
        {
            Radians = 3.14159265358979;
            Points = new List<Point>(20);
            PointsTransformed = new List<Point>(20);
            VelocityX = 0;
            VelocityY = 0;
            CurrLoc = location;
        }

        protected void AddPoint(Point pt)
        {
            Points.Add(pt);
            PointsTransformed.Add(pt);
        }

        public virtual void Draw(ScreenCanvas sc, int x, int y)
        {
            DrawPolyToScreen(PointsTransformed, sc, x, y, Pens.White);
        }

        protected void Draw(ScreenCanvas sc, int x, int y, Pen penColor)
        {
            DrawPolyToScreen(PointsTransformed, sc, x, y, penColor);
        }

        protected void DrawPolyToScreen(List<Point> alPoly, ScreenCanvas sc, int x, int y, Pen penColor)
        {
            var pointArray = new Point[alPoly.Count];
            for (var i = 0; i < alPoly.Count; i++)
            {
                pointArray[i].X = (int)((double)(CurrLoc.X + alPoly[i].X) / 10000 * x);
                pointArray[i].Y = (int)((double)(CurrLoc.Y + alPoly[i].Y) / 7500 * y);
            }
            sc.AddPolygon(pointArray, penColor);
        }

        public Point GetCurrLoc()
        {
            return CurrLoc;
        }

        public double GetRadians()
        {
            return Radians;
        }

        protected static Pen GetRandomFireColor()
        {
            switch (RndGen.Next(3))
            {
                case 0:
                    return Pens.Red;
                case 1:
                    return Pens.Yellow;
                case 2:
                    return Pens.Orange;
                default:
                    return Pens.White;
            }
        }

        public double GetVelocityX()
        {
            return VelocityX;
        }

        public double GetVelocityY()
        {
            return VelocityY;
        }
        
        public virtual bool Move()
        {
            CurrLoc.X += (int) VelocityX;
            CurrLoc.Y += (int) VelocityY;
            if (CurrLoc.X < 0)
                CurrLoc.X = 9999;
            if (CurrLoc.X >= 10000)
                CurrLoc.X = 0;
            if (CurrLoc.Y < 0)
                CurrLoc.Y = 7499;
            if (CurrLoc.Y >= 7500)
                CurrLoc.Y = 0;
            return true;
        }

        protected void Rotate(double degrees)
        {
            Radians += degrees * 0.0174532925 / 60;
            var sin = Math.Sin(Radians);
            var cos = Math.Cos(Radians);
            PointsTransformed.Clear();
            foreach (var item in Points)
            {
                PointsTransformed.Add(new Point((int) (item.X * cos + item.Y * sin),
                    (int) (item.X * sin - item.Y * cos)));
            }
        }
    }
}