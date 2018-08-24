using System;
using Microsoft.Xna.Framework;

namespace Stroids.ai
{
    public static class StimulusBuilder
    {
        /// <summary>
        /// create an approximation of vision, produce an arry of points about the ship looking in 8 cardinal directions
        /// and record an inverse weighted factor for distance to an asteroid.
        /// Add a parameter to the end of the array that indicates if there is a clear shot
        /// </summary>
        internal static float[] Create(AsteroidsGame game)
        {
            var lvl = game.Level;
            var ship = lvl.Ship;
            var rotation = ship.GetRadians();

            var vision = new float[9];
            for (int i = 0; i < 8; i++)
            {
                var addedRotation = i * (Math.PI / 4);
                var radians = rotation + addedRotation;
                var direction = new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));

                direction.Normalize();
                vision[i] = LookInDirection(game, direction);
            }

            // input 9 is true if facing asteroid and can shoot
            if (lvl.CanShoot() && vision[0] > 0)
            {
                vision[8] = 1;
            }
            else
            {
                vision[8] = 0;
            }

            return vision;
        }

        private static float LookInDirection(AsteroidsGame game, Vector2 direction)
        {
            const int height = 7500;
            const int width = 10000;
            var lvl = game.Level;
            var pos = lvl.Ship.GetCurrLoc();

            //look in the direction for a number of steps
            for (int distance = 50; distance <= 2000; distance += 50)
            {
                //look further in the direction
                var position = new Point(pos.X + (int)(direction.X * distance),
                    pos.Y + (int)(direction.Y * distance));

                //wrap the sight lines around the game board
                if (position.Y < 0)
                {
                    position.Y += height;
                }
                else if (position.Y > height)
                {
                    position.Y -= height;
                }

                if (position.X < 0)
                {
                    position.X += width;
                }
                else if (position.X > width)
                {
                    position.X -= width;
                }

                foreach (var a in lvl.AsteroidBelt)
                {
                    if (a.CheckPointInside(position))
                    {
                        return 1f / (distance * (1 / 50f));
                    }
                }
            }
            return 0;
        }
    }
}