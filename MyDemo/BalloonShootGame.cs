using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace BalloonShoot
{
    public class GameSettings
    {
        public const int ScreenWidth = 1600;
        public const int ScreenHeight = 900;
        public const int BalloonSize = 200;
        public const float BaseBalloonSpeed = 100f;
    }

    public class Balloon
    {
        private Texture2D _texture;
        private Texture2D _popTexture;
        private Random _random;
        private Rectangle _position;
        private bool _isPopped;
        private double _popTimer;
        private double _popDisplayDuration;
        private float _speed;
        private int _score;

        public bool IsPopped => _isPopped;
        public Rectangle Position => _position;

        public Balloon(Texture2D texture, Texture2D popTexture, Random random)
        {
            _texture = texture;
            _popTexture = popTexture;
            _random = random;
            _popDisplayDuration = 0.5;
            ResetPosition();
        }

        public void Update(GameTime gameTime)
        {
            if (_isPopped)
            {
                _popTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (_popTimer >= _popDisplayDuration)
                {
                    _isPopped = false;
                    ResetPosition();
                }
            }
            else
            {
                _position.Y -= (int)(_speed * gameTime.ElapsedGameTime.TotalSeconds);
                if (_position.Y + GameSettings.BalloonSize < 0)
                {
                    ResetPosition();
                    _score--;
                }
            }
        }

        public void Pop()
        {
            _isPopped = true;
            _popTimer = 0;
            _score++;
            _speed = GameSettings.BaseBalloonSpeed + _score * 50;
        }

        public void ResetPosition()
        {
            _position = new Rectangle(_random.Next(0, GameSettings.ScreenWidth - GameSettings.BalloonSize),
                                      GameSettings.ScreenHeight, GameSettings.BalloonSize, GameSettings.BalloonSize);
            _speed = GameSettings.BaseBalloonSpeed + _score * 50;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentTexture = _isPopped ? _popTexture : _texture;
            spriteBatch.Draw(currentTexture, _position, Color.White);
        }
    }

    public class Crosshair
    {
        private Texture2D _texture;
        private int _width;
        private int _height;

        public Crosshair(Texture2D texture, int width, int height)
        {
            _texture = texture;
            _width = width;
            _height = height;
        }

        public void Draw(SpriteBatch spriteBatch, MouseState mouseState)
        {
            int centeredX = mouseState.X - (_width / 2);
            int centeredY = mouseState.Y - (_height / 2);
            spriteBatch.Draw(_texture, new Rectangle(centeredX, centeredY, _width, _height), Color.White);
        }
    }

    public class GameScore
    {
        private SpriteFont _font;
        private int _score;

        public GameScore(SpriteFont font)
        {
            _font = font;
            _score = 0;
        }

        public void Increase()
        {
            _score++;
        }

        public void Decrease()
        {
            _score--;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, $"Score: {_score}", new Vector2(0, 0), Color.White);
        }
    }

    public class BalloonShootGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Balloon _balloon;
        private Crosshair _crosshair;
        private GameScore _gameScore;
        private Texture2D _backgroundTexture;
        private bool _mouseReleased = true;
        private MouseState _mouseState;
        private Random _random;

        public BalloonShootGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GameSettings.ScreenWidth,
                PreferredBackBufferHeight = GameSettings.ScreenHeight
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            _random = new Random();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var balloonTexture = Content.Load<Texture2D>("assets/balloon");
            var popTexture = Content.Load<Texture2D>("assets/pop");
            var crosshairTexture = Content.Load<Texture2D>("assets/crosshair");
            var font = Content.Load<SpriteFont>("assets/myfont");
            _backgroundTexture = Content.Load<Texture2D>("assets/bg");

            _balloon = new Balloon(balloonTexture, popTexture, _random);
            _crosshair = new Crosshair(crosshairTexture, 150, 150);
            _gameScore = new GameScore(font);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _mouseState = Mouse.GetState();
            _balloon.Update(gameTime);

            if (_mouseState.LeftButton == ButtonState.Pressed && _mouseReleased)
            {
                if (!_balloon.IsPopped && _balloon.Position.Contains(_mouseState.Position))
                {
                    _balloon.Pop();
                    _gameScore.Increase();
                }
                _mouseReleased = false;
            }

            if (_mouseState.LeftButton == ButtonState.Released)
                _mouseReleased = true;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, GameSettings.ScreenWidth, GameSettings.ScreenHeight), Color.White);
            _balloon.Draw(_spriteBatch);
            _gameScore.Draw(_spriteBatch);
            _crosshair.Draw(_spriteBatch, _mouseState);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

