using System;
using Microsoft.Xna.Framework;

namespace Stroids.ai
{
    public class Player
    {
        private readonly NeuralNet _brain;
        private float? _fitness;
        private int _lifespan;
        private int _score;
        private float _hitRate;

        internal Player() : this(new NeuralNet(9, 16, 4))
        {
        }

        private Player(NeuralNet brain)
        {
            _brain = brain;
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public float HitRate
        {
            get { return _hitRate; }
            set { _hitRate = value; }
        }

        /// <summary>
        /// for genetic algorithm
        /// </summary>
        private float CalculateFitness()
        {
            var fit = Score * 5f;
            fit += _lifespan * 0.8f;

            if(!float.IsInfinity(HitRate))
                fit += HitRate * 2f;
            return fit;
        }

        public float Fitness
        {
            get
            {
                if(_fitness == null)
                    _fitness = CalculateFitness();
                return _fitness.GetValueOrDefault();
            }
        }

        /// <summary>
        /// returns a mutated version of this players brain
        /// </summary>
        /// <returns></returns>
        public Player Mutate()
        {
            return new Player(_brain.Mutate(0.1f));
        }

        /// <summary>
        /// returns a clone of this player with the same brain
        /// </summary>
        /// <returns></returns>
        public Player Clone()
        {
            return new Player(_brain.Clone());
        }

        /// <summary>
        /// Returns a new player with a genetic makeup of two parents
        /// </summary>
        /// <param name="mate"></param>
        /// <returns></returns>
        public Player Crossover(Player mate)
        {
            return new Player(_brain.Crossover(mate._brain));
        }

        /// <summary>
        /// create an approximation of vision, produce an arry of points about the ship looking in 8 cardinal directions
        /// and record an inverse weighted factor for distance to an asteroid.
        /// Add a parameter to the end of the array that indicates if there is a clear shot
        /// </summary>
        internal static float[] CreateStimuli()
        {
            var lvl = AsteroidsGame.Instance.Level;
            var ship = lvl.Ship;
            var rotation = ship.GetRadians();

            var vision = new float[9];
            for (int i = 0; i < 8; i++)
            {
                var addedRotation = i * (Math.PI / 4);
                var radians = rotation + addedRotation;
                var direction = new Vector2((float) Math.Cos(radians), (float) Math.Sin(radians));

                direction.Normalize();
                vision[i] = LookInDirection(direction);
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

        private static float LookInDirection(Vector2 direction)
        {
            var game = AsteroidsGame.Instance;

            const int height = 7500;
            const int width = 10000;
            var lvl = game.Level;
            var pos = lvl.Ship.GetCurrLoc();

            //look in the direction for a number of steps
            for (int distance = 50; distance <= 2000; distance+=50)
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
                        return 1f / (distance * (1/50f));
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// convert the output of the neural network to actions
        /// </summary>
        internal void Respond(float[] stimulus)
        {
            var lvl = AsteroidsGame.Instance.Level;
            //get the output of the neural network
            var output = _brain.Output(stimulus);

            // map first output to thrust 
            lvl.Thrust(output[0] > 0.8);

            if (output[1] > 0.8)
            {
                //output 1 is turn left
                lvl.Left();
            }
            else if (output[2] > 0.8)
            {
                //output 2 is turn right
                lvl.Right();
            }

            //shooting
            if (output[3] > 0.8)
            {
                //output 3 is shooting
                lvl.Shoot();
            }

            //not exactly lifespan but close...
            _lifespan++;
        }
    }
}
