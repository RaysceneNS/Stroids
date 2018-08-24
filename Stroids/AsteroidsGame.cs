using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stroids.ai;
using Stroids.Game;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Stroids
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class AsteroidsGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        private BasicEffect _basicEffect;
        private readonly TitleScreen _currTitle;
        private GameState _gameState;
        private readonly Score _score;
        private readonly ScreenCanvas _screenCanvas;
        private KeyboardState _previousKeyboardState;
        private readonly Population _population;


        internal AsteroidsGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _gameState = GameState.Init;
            _screenCanvas = new ScreenCanvas();
            _score = new Score();
            _currTitle = new TitleScreen();
            _population = new Population(this, 20);

            this.IsFixedTimeStep = true;
        }

        /// <summary>
        /// Returns the current level in the game
        /// </summary>
        internal Level Level { get; private set; }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _gameState = GameState.Title;
            _previousKeyboardState = Keyboard.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
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

            Level = new Level(_score);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            switch (_gameState)
            {
                case GameState.Title:
                {
                    UpdateTitleScreen(keyboardState);
                    break;
                }
                case GameState.Game:
                {
                    UpdateGame(keyboardState);
                    break;
                }
                case GameState.Evolve:
                {
                    UpdateEvolve(keyboardState);
                    break;
                }
            }
            _previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        private void UpdateEvolve(KeyboardState keyboardState)
        {
            //if any players are alive then update them
            _population.UpdateActive();

            var newState = Level.Update(keyboardState, _previousKeyboardState);

            if (newState == GameState.Title)
            {
                if (Level.IsDone())
                {
                    //game over for this player.. 
                    var activePlayer = _population.ActivePlayer();
                    activePlayer.Score = _score.CurrentScore;

                    //select the next in line to try
                    if (!_population.SelectNextPlayer())
                    {
                        //all the players have played their last upon the stage of life,
                        // score each and perform a selection of the fitest players to pass
                        // their genetic material to the next gen.
                        _population.NaturalSelection();
                        //reset the score and let this next gen play
                        _score.Reset();
                        Level.StartGame();
                    }
                    else
                    {

                        //reset the score and let this next gen play
                        _score.Reset();
                        Level.ReStartGame();
                    }
                }
                else
                {
                    //game evolution exited 
                    _gameState = GameState.Title;
                    //transition to title
                    _score.CancelGame();
                    _currTitle.Reset();
                }
            }
        }

        private void UpdateGame(KeyboardState keyboardState)
        {
            _gameState = Level.Update(keyboardState, _previousKeyboardState);
            if (_gameState == GameState.Title)
            {
                //transition to title
                _score.CancelGame();
                _currTitle.Reset();
            }
        }

        private void UpdateTitleScreen(KeyboardState keyboardState)
        {
            var trigger = _gameState;
            // get out of title screen mode by pressing 'fire' or arrow keys
            if (keyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Right) && !_previousKeyboardState.IsKeyDown(Keys.Right) ||
                keyboardState.IsKeyDown(Keys.Left) && !_previousKeyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up))
            {
                trigger = GameState.Game;
            }

            if (keyboardState.IsKeyDown(Keys.E) && !_previousKeyboardState.IsKeyDown(Keys.E))
            {
                trigger = GameState.Evolve;
            }

            if (keyboardState.IsKeyDown(Keys.Escape) && !_previousKeyboardState.IsKeyDown(Keys.Escape))
            {
                trigger = GameState.Exit;
            }
            
            switch (trigger)
            {
                case GameState.Game:
                    //transition from title to game. 
                    _gameState = trigger;
                    _score.Reset();
                    Level.StartGame();
                    break;
                case GameState.Evolve:
                    //transition from title to evolution games. 
                    _gameState = trigger;
                    this.TargetElapsedTime = TimeSpan.FromMilliseconds(1);
                    _score.Reset();
                    Level.StartGame();
                    break;
                case GameState.Exit:
                    //transition to exit;
                    _gameState = trigger;
                    Exit();
                    break;
            }
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
                    Level.Draw(_screenCanvas, width, height);
                    break;
                }
                case GameState.Evolve:
                {
                    Level.Draw(_screenCanvas, width, height);
                    _screenCanvas.AddText($"GEN {_population.Generation}.{_population.ActiveIndex}", Justify.Right, 100, 200, 400, width, height);
                    break;
                }
            }
            
            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
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
    }
}
