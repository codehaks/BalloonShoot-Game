using BalloonShoot.Common;
using BalloonShoot.Models;
using BalloonShoot.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BalloonShoot;
public class BalloonShootGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private List<Balloon> _balloons; // List of balloons
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

        _balloons = new List<Balloon>(); // Initialize the list of balloons
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
        _crosshair = _serviceProvider.GetRequiredService<Crosshair>();
        _gameScore = _serviceProvider.GetRequiredService<GameScore>();
        _gameRenderer = _serviceProvider.GetRequiredService<GameRenderer>();

        // Create multiple balloons and add them to the list
        for (int i = 0; i < 5; i++) // Add 2 balloons
        {
            var balloon = new Balloon(contentLoader.BalloonTexture, contentLoader.PopTexture, _random);
            _balloons.Add(balloon);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _mouseState = Mouse.GetState();

       


        // Update each balloon and check for collisions
        foreach (var balloon in _balloons)
        {
            balloon.Update(gameTime);

            // Check for collision with each balloon
            if (_inputHandler.IsLeftMouseClick(_mouseState) && !balloon.IsPopped && balloon.Position.Contains(_mouseState.Position))
            {
                Debug.WriteLine(_balloons.IndexOf(balloon));
                balloon.Pop();
                _gameScore.Increase();
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();

        // Draw background
        _gameRenderer.DrawBackground();

        // Draw each balloon
        foreach (var balloon in _balloons)
        {
            balloon.Draw(_spriteBatch);
        }

        // Draw other game elements
        _gameScore.Draw(_spriteBatch);
        _crosshair.Draw(_spriteBatch, _mouseState);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}


