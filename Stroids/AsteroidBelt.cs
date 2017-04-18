﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Stroids
{
    internal class AsteroidBelt
    {
        private const int SafeDistance = 2000;
        private List<Asteroid> _asteroids;

        public AsteroidBelt(int numAsteroids)
        {
            StartBelt(numAsteroids, AsteroidSize.Large);
        }

        public AsteroidBelt(int numAsteroids, AsteroidSize minSize)
        {
            StartBelt(numAsteroids, minSize);
        }

        public int CheckPointCollisions(Point ptCheck)
        {
            var num = 0;
            var count = _asteroids.Count - 1;
            while (count >= 0)
            {
                if (!_asteroids[count].CheckPointInside(ptCheck))
                {
                    count--;
                }
                else
                {
                    var size = _asteroids[count].ReduceSize();
                    switch (size)
                    {
                        case AsteroidSize.Dne:
                            num = 250;
                            Sounds.Play(Sound.BangLarge);
                            _asteroids.RemoveAt(count);
                            break;
                        case AsteroidSize.Small:
                            num = 100;
                            Sounds.Play(Sound.BangSmall);
                            break;
                        case AsteroidSize.Medium:
                            num = 50;
                            Sounds.Play(Sound.BangMedium);
                            break;
                    }
                    if (size == AsteroidSize.Dne)
                        break;
                    _asteroids.Add(new Asteroid(_asteroids[count]));
                    break;
                }
            }
            return num;
        }

        public int Count()
        {
            return _asteroids.Count;
        }

        public void Draw(ScreenCanvas sc, int x, int y)
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.Draw(sc, x, y);
            }
        }

        public bool IsCenterSafe()
        {
            var flag = true;
            foreach (var asteroid in _asteroids)
            {
                var currLoc = asteroid.GetCurrLoc();
                if (Math.Sqrt(Math.Pow(currLoc.X - 5000, 2) + Math.Pow(currLoc.Y - 3750, 2)) > SafeDistance)
                    continue;
                flag = false;
                break;
            }
            return flag;
        }

        public void Move()
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.Move();
            }
        }

        private void StartBelt(int numAsteroids, AsteroidSize minSize)
        {
            var rndGen = new Random();

            _asteroids = new List<Asteroid>();
            for (var i = 0; i < numAsteroids; i++)
            {
                var size = (AsteroidSize)3 - rndGen.Next(3 - (int)minSize + 1);
                _asteroids.Add(new Asteroid(size));
            }
        }
    }
}