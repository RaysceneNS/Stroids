using Microsoft.Xna.Framework.Input;

namespace Stroids.Game
{
    internal class TitleScreen
    {
        private readonly AsteroidBelt _asteroids;
        private int _increment;
        private int _letterSize;
        private string _title;

        public TitleScreen()
        {
            Reset();
            _asteroids = new AsteroidBelt(15, AsteroidSize.Small);
        }

        public void Reset()
        {
            _letterSize = 40;
            _increment = 16;
            _title = "READY PLAYER ONE";
        }

        public AsteroidsGame.GameState Update(KeyboardState state, KeyboardState previousState)
        {
            // get out of title screen mode by pressing 'fire' or arrow keys
            if (
                (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space) ||
                 state.IsKeyDown(Keys.Right) && !previousState.IsKeyDown(Keys.Right) ||
                 state.IsKeyDown(Keys.Left) && !previousState.IsKeyDown(Keys.Left) ||
                 state.IsKeyDown(Keys.Up) && !previousState.IsKeyDown(Keys.Up)))
            {
                return AsteroidsGame.GameState.Game;
            }

            if (state.IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
            {
                return AsteroidsGame.GameState.Exit;
            }
            return AsteroidsGame.GameState.Title;
        }

        public void Draw(ScreenCanvas screenCanvas, int width, int height)
        {
            if (_letterSize > 1000 || _letterSize < 40)
            {
                _increment = -_increment;
                if (_letterSize < 40)
                    _title = _title != "GAME OVER" ? "GAME OVER" : "ASTEROIDS";
            }

            _letterSize += _increment;
            screenCanvas.AddText(_title, Justify.Center, 3750 - _letterSize, _letterSize, _letterSize * 2, width, height);

            _asteroids.Move();
            _asteroids.Draw(screenCanvas, width, height);
        }
    }
}