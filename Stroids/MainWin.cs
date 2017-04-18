using System;
using System.ComponentModel;
using System.Timers;
using System.Windows.Forms;

namespace Stroids
{
    public partial class MainWin : Form
    {
        private readonly PictureBox[] _frames;
        private Game _currGame;
        private TitleScreen _currTitle;
        private Modes _gameStatus;
        private bool _lastDrawn;
        private bool _leftPressed;
        private bool _pauseLastPressed;
        private bool _rightPressed;
        private Score _score;
        private ScreenCanvas _screenCanvas;
        private bool _shootingLastPressed;
        private bool _hyperspaceLastPressed;
        private int _showFrame;
        private System.Timers.Timer _timerFlip;
        private bool _upPressed;

        public MainWin()
        {
            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();

            _frames = new[] { frame1, frame2 };
            _showFrame = 0;
            _lastDrawn = false;

            foreach (var pictureBox in _frames)
            {
                pictureBox.Top = 0;
                pictureBox.Left = 0;
                pictureBox.Width = ClientSize.Width;
                pictureBox.Height = ClientSize.Height;
            }
            _gameStatus = Modes.PREP;
        }

        private void FlipDisplay(object source, ElapsedEventArgs e)
        {
            _screenCanvas.Clear();
            switch (_gameStatus)
            {
                case Modes.TITLE:
                {
                    TitleScreen();
                    break;
                }
                case Modes.GAME:
                {
                    if (PlayGame())
                        break;
                    _currTitle = new TitleScreen();
                    _currTitle.InitTitleScreen();
                    break;
                }
            }

            _lastDrawn = false;
            _frames[_showFrame].BringToFront();
            _frames[_showFrame].Visible = true;
            
            _showFrame = 1 - _showFrame;
            _frames[_showFrame].Visible = false;
            if (_gameStatus != Modes.EXIT)
                SetFlipTimer();
        }

        private void Frame_Paint(object sender, PaintEventArgs e)
        {
            if (_lastDrawn) 
                return;
            _lastDrawn = true;
            _screenCanvas.Draw(e.Graphics);
        }
        
        private void MainWin_Activated(object sender, EventArgs e)
        {
            if (_gameStatus != Modes.PREP)
                return;

            _screenCanvas = new ScreenCanvas();
            _score = new Score();
            _gameStatus = Modes.TITLE;
            _currTitle = new TitleScreen();
            _currTitle.InitTitleScreen();
            SetFlipTimer();
        }

        private void MainWin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                switch (_gameStatus)
                {
                    case Modes.GAME:
                        _score.CancelGame();
                        _currTitle = new TitleScreen();
                        _gameStatus = Modes.TITLE;
                        break;
                    case Modes.TITLE:
                        _gameStatus = Modes.EXIT;
                        Close();
                        break;
                }
            }
            else
            {
                if (_gameStatus == Modes.TITLE)
                {
                    _score.ResetGame();
                    _currGame = new Game();
                    _gameStatus = Modes.GAME;
                    _leftPressed = false;
                    _rightPressed = false;
                    _upPressed = false;
                    _hyperspaceLastPressed = false;
                    _shootingLastPressed = false;
                    _pauseLastPressed = false;
                    return;
                }

                if (e.KeyData == Keys.Left)
                    _leftPressed = true;

                if (e.KeyData == Keys.Right)
                    _rightPressed = true;

                if (e.KeyData == Keys.Up)
                    _upPressed = true;

                if (!_hyperspaceLastPressed && e.KeyData == Keys.Down)
                {
                    _hyperspaceLastPressed = true;
                    _currGame.HyperspaceJump();
                }

                if (!_shootingLastPressed && e.KeyData == Keys.Space)
                {
                    _shootingLastPressed = true;
                    _currGame.Shoot();
                }

                if (!_pauseLastPressed && e.KeyData == Keys.P)
                {
                    _pauseLastPressed = true;
                    _currGame.Pause();
                }
            }
        }

        private void MainWin_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    _leftPressed = false;
                    break;

                case Keys.Right:
                    _rightPressed = false;
                    break;

                case Keys.Up:
                    _upPressed = false;
                    break;

                case Keys.Down:
                    _hyperspaceLastPressed = false;
                    break;

                case Keys.Space:
                    _shootingLastPressed = false;
                    break;

                case Keys.P:
                    _pauseLastPressed = false;
                    break;
            }
        }

        private void MainWin_Resize(object sender, EventArgs e)
        {
            foreach (var box in _frames)
            {
                box.Width = ClientSize.Width;
                box.Height = ClientSize.Height;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _gameStatus = Modes.EXIT;
            base.OnClosing(e);
        }

        private bool PlayGame()
        {
            if (_leftPressed)
                _currGame.Left();
            if (_rightPressed)
                _currGame.Right();
            _currGame.Thrust(_upPressed);
            
            _currGame.Draw(_screenCanvas, ClientSize.Width, ClientSize.Height, ref _score);
            if (_currGame.Done())
                _gameStatus = Modes.TITLE;
            return _gameStatus == Modes.GAME;
        }

        private void SetFlipTimer()
        {
            _timerFlip = new System.Timers.Timer(16.6666666666667);
            _timerFlip.Elapsed += FlipDisplay;
            _timerFlip.AutoReset = false;
            _timerFlip.Enabled = true;
        }

        private void TitleScreen()
        {
            _score.Draw(_screenCanvas, ClientSize.Width, ClientSize.Height);
            _currTitle.Draw(_screenCanvas, ClientSize.Width, ClientSize.Height);
        }
    }
}