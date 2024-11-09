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
        services.AddSingleton<Balloon>(provider =>
        {
            var contentLoader = provider.GetRequiredService<ContentLoader>();
            return new Balloon(contentLoader.BalloonTexture, contentLoader.PopTexture, provider.GetRequiredService<Random>());
        });
        services.AddSingleton<Crosshair>(provider =>
        {
            var contentLoader = provider.GetRequiredService<ContentLoader>();
            return new Crosshair(contentLoader.CrosshairTexture, 150, 150);
        });
        services.AddSingleton<GameScore>(provider =>
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

        // Resolve and load content
        var contentLoader = _serviceProvider.GetRequiredService<ContentLoader>();
        contentLoader.LoadAllContent();

        // Retrieve instances from the service provider
        _balloon = _serviceProvider.GetRequiredService<Balloon>();
        _crosshair = _serviceProvider.GetRequiredService<Crosshair>();
        _gameScore = _serviceProvider.GetRequiredService<GameScore>();
        _gameRenderer = _serviceProvider.GetRequiredService<GameRenderer>();
    }


    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _mouseState = Mouse.GetState();

        // Update the balloon and check for collision
        var balloon = _serviceProvider.GetRequiredService<Balloon>();
        balloon.Update(gameTime);

        var inputHandler = _serviceProvider.GetRequiredService<InputHandler>();
        if (inputHandler.IsLeftMouseClick(_mouseState) && !balloon.IsPopped && balloon.Position.Contains(_mouseState.Position))
        {
            balloon.Pop();
            var gameScore = _serviceProvider.GetRequiredService<GameScore>();
            gameScore.Increase();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();

        // Draw background and game elements
        var gameRenderer = _serviceProvider.GetRequiredService<GameRenderer>();
        gameRenderer.DrawBackground();
        gameRenderer.DrawGameElements(_serviceProvider.GetRequiredService<Balloon>(),
                                      _serviceProvider.GetRequiredService<Crosshair>(),
                                      _serviceProvider.GetRequiredService<GameScore>(),
                                      _mouseState);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}


