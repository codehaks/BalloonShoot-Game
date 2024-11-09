using BalloonShoot.Common;
using BalloonShoot.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BalloonShoot;


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

