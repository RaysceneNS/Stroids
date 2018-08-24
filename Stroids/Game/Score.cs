using System;

namespace Stroids.Game
{
    public class Score
    {
        private const int FreeShipIncrement = 10000;
        private int _freeShip;
        internal event Action OnExtraShip;
        
        public Score()
        {
            Ships = 0;
            CurrentScore = 0;
            HiScore = 0;
            _freeShip = FreeShipIncrement;
        }

        public int Ships { get; private set; }

        public int HiScore { get; private set; }

        public int CurrentScore { get; private set; }

        public int ShotsHit { get; private set; }

        public void AddScore(int score)
        {
            ShotsHit = ShotsHit + 1;

            CurrentScore += score;
            if (CurrentScore >= _freeShip)
            {
                Ships++;
                _freeShip += FreeShipIncrement;
                OnExtraShip?.Invoke();
            }
            if (CurrentScore >= 1000000)
                CurrentScore = CurrentScore % 1000000;
            if (CurrentScore > HiScore)
                HiScore = CurrentScore;
        }

        public void CancelGame()
        {
            Ships = 0;
        }

        public void UseNewShip()
        {
            Ships--;
        }

        public bool HasReserveShips()
        {
            return Ships > 1;
        }

        public void Reset()
        {
            Ships = 3;
            CurrentScore = 0;
            _freeShip = FreeShipIncrement;
        }
    }
}
