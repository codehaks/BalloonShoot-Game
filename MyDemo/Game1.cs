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
        SpriteFont _gameFont;

        int _balloonX = 100;
        int _balloonY = 100;

        Rectangle balloonPosition;
        const int balloonSize = 200;

        // Assuming _CrosshairSprite is the texture, and mouseState represents the current mouse state.
        int spriteWidth = 150; // Width of your sprite
        int spriteHeight = 150; // Height of your sprite

        int _score = 0;

        bool _mouseReleased = true;


        MouseState mouseState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            balloonPosition = new Rectangle(_balloonX, _balloonY, balloonSize, balloonSize);
            IsMouseVisible = false;
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
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && _mouseReleased)
            {
                var targetCenter = new Vector2(balloonPosition.X+(balloonSize/ 2), balloonPosition.Y+ (balloonSize ) / 2);
                var distanceFromCenter = Math.Sqrt(Math.Pow((mouseState.X- targetCenter.X), 2) + Math.Pow((mouseState.Y - targetCenter.Y), 2));
                if (distanceFromCenter <= (balloonSize/4))
                {
                    _score++;
                }
                _mouseReleased = false;
            }


            if (mouseState.LeftButton == ButtonState.Released)
            {
                _mouseReleased = true;
            }

            // balloonPosition.
            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(_bgSprite, new Vector2(0, 0), Color.AliceBlue);
            _spriteBatch.Draw(_balloonSprite, balloonPosition, Color.AliceBlue);
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
