namespace Stroids.ai
{
    public class Player
    {
        private readonly NeuralNet _brain;
        private float? _fitness;
        private bool _moved;
        private bool _thrust;

        internal Player()
        {
            _brain = new NeuralNet(9, 16, 4);
        }

        private Player(NeuralNet brain)
        {
            _brain = brain;
        }

        public int Score { get; set; }

        /// <summary>
        /// for genetic algorithm
        /// </summary>
        private float CalculateFitness()
        {
            var fit = Score * 1.5f;

            if (_moved)
            {
                fit *= 2;
            }

            if (_thrust)
            {
                fit *= 2;
            }

            return fit;
        }

        public float Fitness
        {
            get
            {
                if (_fitness == null)
                {
                    _fitness = CalculateFitness();
                }
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
        /// convert the output of the neural network to actions
        /// </summary>
        internal void Respond(AsteroidsGame game, float[] stimulus)
        {
            //get the output of the neural network
            var output = _brain.Output(stimulus);

            // map first output to thrust 
            var lvl = game.Level;
            if (output[0] > 0.8)
            {
                lvl.Thrust(true);
                _thrust = true;
            }
            else
            {
                lvl.Thrust(false);
            }

            if (output[1] > 0.8)
            {
                //output 1 is turn left
                lvl.Left();
                _moved = true;
            }
            if (output[2] > 0.8)
            {
                //output 2 is turn right
                lvl.Right();
                _moved = true;
            }

            //shooting
            if (output[3] > 0.8)
            {
                //output 3 is shooting
                lvl.Shoot();
            }
        }
    }
}
