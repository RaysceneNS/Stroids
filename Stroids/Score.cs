namespace Stroids
{
    public class Score
    {
        private const int FreeShipIncrement = 10000;
        private int _freeShip;
        private int _hiScore;
        private int _score;
        private int _ships;

        public Score()
        {
            _ships = 0;
            _score = 0;
            _hiScore = 0;
            _freeShip = FreeShipIncrement;
        }

        public void AddScore(int iAddScore)
        {
            _score += iAddScore;
            if (_score >= _freeShip)
            {
                _ships ++;
                _freeShip += FreeShipIncrement;
                Sounds.Play(Sound.ExtraShip);
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

        public void Draw(ScreenCanvas screenCanvas, int iPictX, int iPictY)
        {
            var str = string.Concat(_score.ToString("000000"), " ");
            if (_ships <= 10)
            {
                for (var i = 0; i < _ships - 1; i++)
                {
                    str = string.Concat(str, "^");
                }
            }
            else
            {
                str = string.Concat(str, "^x", _ships - 1);
            }
            screenCanvas.AddText(str, Justify.Left, 100, 200, 400, iPictX, iPictY);

            screenCanvas.AddText(_hiScore.ToString("000000"), Justify.Center, 100, 200, 400, iPictX, iPictY);
        }

        public void GetNewShip()
        {
            _ships--;
        }

        public bool HasReserveShips()
        {
            return _ships > 1;
        }

        public void ResetGame()
        {
            _ships = 3;
            _score = 0;
            _freeShip = FreeShipIncrement;
        }
    }
}