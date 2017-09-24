using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Stroids.Game
{
    public class Explosions
    {
        private readonly List<Explosion> _explosions;

        public Explosions()
        {
            _explosions = new List<Explosion>();
        }

        public void AddExplosion(Point ptExplosion, double timeFactor = 1)
        {
            _explosions.Add(new Explosion(ptExplosion, timeFactor));
        }

        public int Count()
        {
            return _explosions.Count;
        }

        public void Draw(ScreenCanvas sc, int x, int y)
        {
            foreach (var explosion in _explosions)
            {
                explosion.Draw(sc, x, y);
            }
        }

        public void Move()
        {
            for (var i = _explosions.Count - 1; i >= 0; i--)
            {
                if (!_explosions[i].Move())
                {
                    _explosions.RemoveAt(i);
                }
            }
        }
    }
}
