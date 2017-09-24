using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stroids.Game;

namespace Stroids
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class AsteroidsGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        
        private readonly TitleScreen _currTitle;
        private Level _level;

        private GameState _gameState;
        private readonly Score _score;
        private readonly ScreenCanvas _screenCanvas;

        private KeyboardState _previousState;

        public AsteroidsGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //_graphics.IsFullScreen = true;
            _gameState = GameState.Init;
            _screenCanvas = new ScreenCanvas();
            _score = new Score();
            _currTitle = new TitleScreen();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _gameState = GameState.Title;
            _previousState = Keyboard.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _basicEffect = new BasicEffect(GraphicsDevice)
            {
                World = Matrix.Identity,
                View = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up),
                // Specify how 3D points are projected/transformed onto the 2D screen
                Projection = Matrix.CreateOrthographicOffCenter(0,
                    GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height,
                    0,
                    1.0f, 1000.0f),
                // Tell BasicEffect to make use of your vertex colors
                VertexColorEnabled = true
            };

            _level = new Level(Services, _score);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Poll for current keyboard state
            KeyboardState state = Keyboard.GetState();

            switch (_gameState)
            {
                case GameState.Title:
                {
                    _gameState = _currTitle.Update(state, _previousState);
                    if (_gameState == GameState.Game)
                    {
                        //transition from title to game. 
                        _score.Reset();
                        _level.StartGame();
                    }
                    if (_gameState == GameState.Exit)
                    {
                        //transition to exit;
                        Exit();
                    }
                    break;
                }
                case GameState.Game:
                {
                    _gameState = _level.Update(state, _previousState);
                    if (_gameState == GameState.Title)
                    {
                        //transition to title
                        _score.CancelGame();
                        _currTitle.Reset();
                    }
                    break;
                }
            }
            
            _previousState = state;
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.BlendState = BlendState.Opaque;
            
            // I'm setting this so that *both* sides of triangle are drawn
            // (so it won't be back-face culled if you move it, or the camera around behind it)
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            var width = this.Window.ClientBounds.Width;
            var height = this.Window.ClientBounds.Height;

            _screenCanvas.Clear();
            DrawHud(width, height);
            switch (_gameState)
            {
                case GameState.Title:
                {
                    _currTitle.Draw(_screenCanvas, width, height);
                    break;
                }
                case GameState.Game:
                {
                    _level.Draw(_screenCanvas, width, height);
                    break;
                }
            }
            
            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //move screen canvas to graphics device
                _screenCanvas.Draw(GraphicsDevice);
            }
            base.Draw(gameTime);
        }

        private void DrawHud(int width, int height)
        {
            var str = string.Concat(_score.CurrentScore.ToString("000000"), " ");
            if (_score.Ships <= 10)
            {
                for (var i = 0; i < _score.Ships - 1; i++)
                {
                    str = string.Concat(str, "^");
                }
            }
            else
            {
                str = string.Concat(str, "^x", _score.Ships - 1);
            }
            _screenCanvas.AddText(str, Justify.Left, 100, 200, 400, width, height);
            _screenCanvas.AddText(_score.HiScore.ToString("000000"), Justify.Center, 100, 200, 400, width, height);
        }

        internal enum GameState
        {
            Init,
            Title,
            Game,
            Exit
        }
    }
}
