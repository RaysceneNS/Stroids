namespace Stroids
{
    public class TitleScreen
    {
        private readonly AsteroidBelt _asteroids;
        private int _increment;
        private int _letterSize;
        private string _title;

        public TitleScreen()
        {
            InitTitleScreen();
            _asteroids = new AsteroidBelt(15, AsteroidSize.Small);
        }

        public void Draw(ScreenCanvas screenCanvas, int x, int y)
        {
            if (_letterSize > 1000 || _letterSize < 40)
            {
                _increment = -_increment;
                if (_letterSize < 40)
                    _title = _title != "GAME OVER" ? "GAME OVER" : "ASTEROIDS";
            }

            _letterSize += _increment;
            screenCanvas.AddText(_title, Justify.Center, 3750 - _letterSize, _letterSize, _letterSize * 2, x, y);
            screenCanvas.AddText("ALL YOUR BASE", Justify.Center, 6900, 200, 400, x, y);
            _asteroids.Move();
            _asteroids.Draw(screenCanvas, x, y);
        }

        public void InitTitleScreen()
        {
            _letterSize = 40;
            _increment = 16;
            _title = "READY PLAYER ONE";
        }
    }
}