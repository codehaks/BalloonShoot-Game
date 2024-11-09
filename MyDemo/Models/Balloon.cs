using BalloonShoot.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace BalloonShoot
{
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
}

