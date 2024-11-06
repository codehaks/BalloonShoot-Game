using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyDemo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D _balloonSprite;
        Texture2D _CrosshairSprite;
        Texture2D _bgSprite;
        Texture2D _popSprite;

        SpriteFont _gameFont;

        int _balloonX = 100;
        int _balloonY = 100;

        Rectangle balloonPosition;
        const int balloonSize = 200;

        int spriteWidth = 150; // Width of your sprite
        int spriteHeight = 150; // Height of your sprite

        int _score = 0;

        bool _mouseReleased = true;

        bool _balloonPopped = false;
        double _popDisplayDuration = 0.5; // Pop display duration in seconds
        double _popTimer = 0; // Timer to track how long the pop texture is displayed

        float _balloonSpeed = 300f;  // Speed of the balloon in pixels per second

        MouseState mouseState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            // Set desired screen size
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;

            _graphics.ApplyChanges(); // Apply the new screen size settings

            Content.RootDirectory = "Content";
            balloonPosition = new Rectangle(_balloonX, _balloonY, balloonSize, balloonSize);
            IsMouseVisible = false;
            random = new Random();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _balloonSprite = Content.Load<Texture2D>("assets/balloon");
            _bgSprite = Content.Load<Texture2D>("assets/bg");
            _CrosshairSprite = Content.Load<Texture2D>("assets/crosshair");
            _gameFont = Content.Load<SpriteFont>("assets/myfont");
            _popSprite = Content.Load<Texture2D>("assets/pop");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            mouseState = Mouse.GetState();

            if (_balloonPopped)
            {
                // If the balloon is popped, increment the timer
                _popTimer += gameTime.ElapsedGameTime.TotalSeconds;

                // If the timer exceeds the display duration, reset the balloon
                if (_popTimer >= _popDisplayDuration)
                {
                    _balloonPopped = false;
                    SetRandomBalloonPosition();
                    _popTimer = 0;
                }
            }
            else
            {
                // Move the balloon upwards
                _balloonY -= (int)(_balloonSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                balloonPosition.Y = _balloonY;

                // Check if the balloon has moved off the screen at the top
                if (_balloonY + balloonSize < 0)
                {
                    // Balloon missed, decrease score and reset position
                    _score--;
                    SetRandomBalloonPosition();
                }

                // Check if the mouse is clicking and hit the balloon
                if (mouseState.LeftButton == ButtonState.Pressed && _mouseReleased)
                {
                    var targetCenter = new Vector2(balloonPosition.X + (balloonSize / 2), balloonPosition.Y + (balloonSize / 2));
                    var distanceFromCenter = Math.Sqrt(Math.Pow((mouseState.X - targetCenter.X), 2) + Math.Pow((mouseState.Y - targetCenter.Y), 2));

                    if (distanceFromCenter <= (balloonSize / 2))
                    {
                        _score++;
                        _balloonPopped = true; // Set balloon as popped
                    }
                    _mouseReleased = false;
                }

                if (mouseState.LeftButton == ButtonState.Released)
                {
                    _mouseReleased = true;
                }
            }

            _balloonSpeed = 100 + _score * 50;


            base.Update(gameTime);
        }
        Random random;
        private void SetRandomBalloonPosition()
        {
            int maxX = _graphics.PreferredBackBufferWidth - balloonSize;

            // Set a random X position, but reset Y to the bottom of the screen
            _balloonX = random.Next(0, maxX);
            _balloonY = _graphics.PreferredBackBufferHeight;

            balloonPosition = new Rectangle(_balloonX, _balloonY, balloonSize, balloonSize);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(_bgSprite, new Vector2(0, 0), Color.AliceBlue);

            if (_balloonPopped)
            {
                // Draw the pop texture where the balloon was
                _spriteBatch.Draw(_popSprite, balloonPosition, Color.AliceBlue);
            }
            else
            {
                // Draw the balloon if it hasn't been popped
                _spriteBatch.Draw(_balloonSprite, balloonPosition, Color.AliceBlue);
            }



            _spriteBatch.DrawString(_gameFont, $"Score: {_score}", new Vector2(0, 0), Color.White);

            // Adjust mouse position to center the sprite
            int centeredX = mouseState.X - (spriteWidth / 2);
            int centeredY = mouseState.Y - (spriteHeight / 2);

            _spriteBatch.Draw(_CrosshairSprite, new Rectangle(centeredX, centeredY, spriteWidth, spriteHeight), Color.AliceBlue);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
