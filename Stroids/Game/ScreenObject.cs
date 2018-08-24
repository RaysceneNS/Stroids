using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Stroids.Game
{
    [Serializable]
    internal class ScreenObject : ISerializable
    {
        protected readonly List<Point> Points;
        public readonly List<Point> PointsTransformed;
        protected Point CurrLoc;
        protected float Radians;
        protected float VelocityX;
        protected float VelocityY;
        protected static readonly Random RndGen;

        static ScreenObject()
        {
            RndGen = new Random();
        }

        protected ScreenObject(Point location)
        {
            Radians = 3.14159265358979f;
            Points = new List<Point>(20);
            PointsTransformed = new List<Point>(20);
            VelocityX = 0;
            VelocityY = 0;
            CurrLoc = location;
        }

        public ScreenObject(SerializationInfo info, StreamingContext context)
        {
            Radians = (float)info.GetValue("rads", typeof(float));
            Points = new List<Point>(20);
            PointsTransformed = new List<Point>(20);

            int len = info.GetInt32("count");

            for (int i = 0; i < len; i++)
            {
                int x = info.GetInt32("ptX" + i);
                int y = info.GetInt32("ptY" + i);
                var pt = new Point(x,y);
                AddPoint(pt);
            }

            VelocityX = (float)info.GetValue("velocityX", typeof(float));
            VelocityY = (float)info.GetValue("velocityY", typeof(float));

            CurrLoc = new Point( (int)info.GetValue("currLocX", typeof(int)), (int)info.GetValue("currLocY", typeof(int)));
        }

        protected void AddPoint(Point pt)
        {
            Points.Add(pt);
            PointsTransformed.Add(pt);
        }

        public virtual void Draw(ScreenCanvas sc, int x, int y)
        {
            DrawPolyToScreen(PointsTransformed, sc, x, y, Color.White);
        }

        protected void Draw(ScreenCanvas sc, int x, int y, Color penColor)
        {
            DrawPolyToScreen(PointsTransformed, sc, x, y, penColor);
        }

        protected void DrawPolyToScreen(List<Point> alPoly, ScreenCanvas sc, int x, int y, Color penColor)
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

        public float GetRadians()
        {
            return Radians;
        }

        protected static Color GetRandomFireColor()
        {
            switch (RndGen.Next(3))
            {
                case 0:
                    return Color.Red;
                case 1:
                    return Color.Yellow;
                case 2:
                    return Color.Orange;
                default:
                    return Color.White;
            }
        }

        public float GetVelocityX()
        {
            return VelocityX;
        }

        public float GetVelocityY()
        {
            return VelocityY;
        }

        public virtual bool Move()
        {
            CurrLoc.X += (int)VelocityX;
            CurrLoc.Y += (int)VelocityY;
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

        protected void Rotate(float degrees)
        {
            Radians += degrees * 0.0174532925f / 60;
            var sin = Math.Sin(Radians);
            var cos = Math.Cos(Radians);
            PointsTransformed.Clear();
            foreach (var item in Points)
            {
                PointsTransformed.Add(new Point((int)(item.X * cos + item.Y * sin),
                    (int)(item.X * sin - item.Y * cos)));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("rads", Radians);

            info.AddValue("count", Points.Count);
            for (int i = 0; i < Points.Count; i++)
            {
                var pt = Points[i];
                info.AddValue("ptX"+i, pt.X);
                info.AddValue("ptY"+i, pt.Y);
            }

            //info.AddValue("transformed", PointsTransformed);

            info.AddValue("velocityX", VelocityX);
            info.AddValue("velocityY", VelocityY);

            info.AddValue("currLocX", CurrLoc.X);
            info.AddValue("currLocY", CurrLoc.Y);
        }
    }
}