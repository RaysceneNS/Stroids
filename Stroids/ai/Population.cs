using System;

namespace Stroids.ai
{
    public class Population
    {
        private readonly AsteroidsGame _asteroidsGame;
        private Player[] _players;//all the players
        private int _bestPlayerNo;//the position in the array that the best player of this generation is in
        private int _allTimeBestScore;//the score of the best ever player
        private Player _allTimeBestPlayer;
        private int _generation;
        private int _activeIndex;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="asteroidsGame"></param>
        /// <param name="size"></param>
        internal Population(AsteroidsGame asteroidsGame, int size)
        {
            _asteroidsGame = asteroidsGame;
            _players = new Player[size];
            _players[0] = new Player();
        }

        /// <summary>
        /// Update the current player 
        /// </summary>
        public void UpdateActive()
        {
            var activePlayer = _players[_activeIndex];
            activePlayer.Respond(_asteroidsGame, StimulusBuilder.Create(_asteroidsGame)); 
        }

        public Player ActivePlayer()
        {
            return _players[_activeIndex];
        }

        public bool SelectNextPlayer()
        {
            if (IsDone())
            {
                return false;
            }

            //generate a new random ai
            _players[_activeIndex + 1] = new Player();

            _activeIndex++;
            return true;
        }

        private bool IsDone()
        {
            return _activeIndex + 1 == _players.Length;
        }

        public int Generation
        {
            get { return _generation; }
        }

        public int ActiveIndex
        {
            get { return _activeIndex; }
        }

        /// <summary>
        /// sets the best player globally and for this gen
        /// </summary>
        private void SetBestPlayer()
        {
            //get max fitness
            float max = 0;
            int maxIndex = 0;
            for (int i = 0; i < _players.Length; i++)
            {
                if (_players[i].Fitness > max)
                {
                    max = _players[i].Fitness;
                    maxIndex = i;
                }
            }

            _bestPlayerNo = maxIndex;

            //if best this gen is better than the global best score then set the global best as the best this gen
            if (_players[_bestPlayerNo].Score > _allTimeBestScore)
            {
                _allTimeBestScore = _players[_bestPlayerNo].Score;
                _allTimeBestPlayer = _players[_bestPlayerNo].Clone();
            }
        }
        
        /// <summary>
        /// creates the next generation of players by natural selection
        /// </summary>
        public void NaturalSelection()
        {
            var newPlayers = new Player[_players.Length];//Create new players array for the next generation

            SetBestPlayer();//set which player is the best

            //add the best player of this generation to the next generation without mutation
            newPlayers[0] = _players[_bestPlayerNo].Clone();

            for (int i = 1; i < _players.Length; i++)
            {
                //for each remaining spot in the next generation
                var newPlayer = i < _players.Length / 2
                    ? SelectFitPlayer().Clone()
                    : SelectFitPlayer().Crossover(SelectFitPlayer());
                newPlayers[i] = newPlayer.Mutate();
            }

            _players = newPlayers;
            _generation++;
            _activeIndex = 0;
        }

        /// <summary>
        /// chooses player from the population to return randomly(considering fitness)
        /// </summary>
        /// <returns></returns>
        private Player SelectFitPlayer()
        {
            //this function works by randomly choosing a value between 0 and the sum of all the fitness
            //then go through all the players and add their fitness to a running sum and if that sum is greater than the random value generated that player is chosen
            //since players with a higher fitness function add more to the running sum then they have a higher chance of being chosen

            //calculate the sum of all the fitness values
            float fitnessSum = 0;
            foreach (var t in _players)
            {
                fitnessSum += t.Fitness;
            }

            var r = new Random();
            int rand = (int) Math.Floor((decimal) r.Next((int) fitnessSum));

            //sum is the current fitness sum
            int runningSum = 0;
            foreach (var t in _players)
            {
                runningSum += (int)t.Fitness;
                if (runningSum > rand)
                {
                    return t;
                }
            }
            return _players[0];
        }
    }
}
