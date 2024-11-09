using BalloonShoot.Common;
using BalloonShoot.Models;
using BalloonShoot.Rendering;
using System;

namespace BalloonShoot;
public class BalloonShootGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Balloon _balloon;
    private Crosshair _crosshair;
    private GameScore _gameScore;
    private GameRenderer _gameRenderer;
    private InputHandler _inputHandler;
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
        _inputHandler = new InputHandler();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        var balloonTexture = Content.Load<Texture2D>("assets/balloon");
        var popTexture = Content.Load<Texture2D>("assets/pop");
        var crosshairTexture = Content.Load<Texture2D>("assets/crosshair");
        var font = Content.Load<SpriteFont>("assets/myfont");
        var backgroundTexture = Content.Load<Texture2D>("assets/bg");

        _balloon = new Balloon(balloonTexture, popTexture, _random);
        _crosshair = new Crosshair(crosshairTexture, 150, 150);
        _gameScore = new GameScore(font);
        _gameRenderer = new GameRenderer(_spriteBatch, backgroundTexture);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _mouseState = Mouse.GetState();
        _balloon.Update(gameTime);

        if (_inputHandler.IsLeftMouseClick(_mouseState) && !_balloon.IsPopped && _balloon.Position.Contains(_mouseState.Position))
        {
            _balloon.Pop();
            _gameScore.Increase();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();

        _gameRenderer.DrawBackground();
        _gameRenderer.DrawGameElements(_balloon, _crosshair, _gameScore, _mouseState);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}


