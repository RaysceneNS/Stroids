using System;

namespace Stroids.ai
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a pseudo random number with gaussian distribution
        /// </summary>
        /// <param name="r"></param>
        /// <param name="mu">The mean</param>
        /// <param name="sigma">The standard deviation</param>
        /// <returns></returns>
        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            var randNormal = mu + sigma * randStdNormal;

            return randNormal;
        }
    }
}