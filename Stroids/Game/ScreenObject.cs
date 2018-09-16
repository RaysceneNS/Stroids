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
        protected Point CurrentLocation;
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
            CurrentLocation = location;
        }

        protected ScreenObject(SerializationInfo info, StreamingContext context)
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

            CurrentLocation = new Point( (int)info.GetValue("currLocX", typeof(int)), (int)info.GetValue("currLocY", typeof(int)));
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
                pointArray[i].X = (int)((double)(CurrentLocation.X + alPoly[i].X) / 10000 * x);
                pointArray[i].Y = (int)((double)(CurrentLocation.Y + alPoly[i].Y) / 7500 * y);
            }
            sc.AddPolygon(pointArray, penColor);
        }

        public Point GetCurrentLocation()
        {
            return CurrentLocation;
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
            CurrentLocation.X += (int)VelocityX;
            CurrentLocation.Y += (int)VelocityY;
            if (CurrentLocation.X < 0)
                CurrentLocation.X = 9999;
            if (CurrentLocation.X >= 10000)
                CurrentLocation.X = 0;
            if (CurrentLocation.Y < 0)
                CurrentLocation.Y = 7499;
            if (CurrentLocation.Y >= 7500)
                CurrentLocation.Y = 0;
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

            info.AddValue("currLocX", CurrentLocation.X);
            info.AddValue("currLocY", CurrentLocation.Y);
        }
    }
}