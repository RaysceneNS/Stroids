using System;

namespace Stroids.Game
{
    public class Score
    {
        private const int FreeShipIncrement = 10000;
        private int _freeShip;
        private int _hiScore;
        private int _score;
        private int _ships;
        internal event Action OnExtraShip;


        public Score()
        {
            _ships = 0;
            _score = 0;
            _hiScore = 0;
            _freeShip = FreeShipIncrement;
        }

        public int Ships
        {
            get { return _ships; }
        }
        public int HiScore
        {
            get { return _hiScore; }
        }
        public int CurrentScore
        {
            get { return _score; }
        }

        public void AddScore(int score)
        {
            _score += score;
            if (_score >= _freeShip)
            {
                _ships++;
                _freeShip += FreeShipIncrement;
                OnExtraShip?.Invoke();
            }
            if (_score >= 1000000)
                _score = _score % 1000000;
            if (_score > _hiScore)
                _hiScore = _score;
        }

        public void CancelGame()
        {
            _ships = 0;
        }

        public void UseNewShip()
        {
            _ships--;
        }

        public bool HasReserveShips()
        {
            return _ships > 1;
        }

        public void Reset()
        {
            _ships = 3;
            _score = 0;
            _freeShip = FreeShipIncrement;
        }
    }
}
