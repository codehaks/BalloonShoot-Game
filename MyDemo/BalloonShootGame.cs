using BalloonShoot.Common;
using BalloonShoot.Models;
using BalloonShoot.Rendering;
using Microsoft.Extensions.DependencyInjection;
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
    private ContentLoader _contentLoader;
    private ServiceProvider _serviceProvider;


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

        ConfigureServices();
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register basic services
        services.AddSingleton<Random>();
        services.AddSingleton<ContentLoader>(provider => new ContentLoader(Content));
        services.AddSingleton<InputHandler>();

        // Register game components with dependencies
        services.AddTransient<Balloon>(provider =>
        {
            var contentLoader = provider.GetRequiredService<ContentLoader>();
            return new Balloon(contentLoader.BalloonTexture, contentLoader.PopTexture, provider.GetRequiredService<Random>());
        });
        services.AddTransient<Crosshair>(provider =>
        {
            var contentLoader = provider.GetRequiredService<ContentLoader>();
            return new Crosshair(contentLoader.CrosshairTexture, 150, 150);
        });
        services.AddTransient<GameScore>(provider =>
        {
            var contentLoader = provider.GetRequiredService<ContentLoader>();
            return new GameScore(contentLoader.GameFont);
        });
        services.AddSingleton<GameRenderer>(provider =>
        {
            var contentLoader = provider.GetRequiredService<ContentLoader>();
            return new GameRenderer(_spriteBatch, contentLoader.BackgroundTexture);
        });

        // Build the service provider
        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Initialize and load content through ContentLoader
        _contentLoader = new ContentLoader(Content);
        _contentLoader.LoadAllContent();

        // Inject dependencies into game objects
        _balloon = new Balloon(_contentLoader.BalloonTexture, _contentLoader.PopTexture, _random);
        _crosshair = new Crosshair(_contentLoader.CrosshairTexture, 150, 150);
        _gameScore = new GameScore(_contentLoader.GameFont);
        _gameRenderer = new GameRenderer(_spriteBatch, _contentLoader.BackgroundTexture);
        _inputHandler = new InputHandler();
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


