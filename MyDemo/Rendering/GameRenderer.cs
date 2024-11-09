using BalloonShoot.Common;
using BalloonShoot.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BalloonShoot.Rendering;
public class GameRenderer
{
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _backgroundTexture;

    public GameRenderer(SpriteBatch spriteBatch, Texture2D backgroundTexture)
    {
        _spriteBatch = spriteBatch;
        _backgroundTexture = backgroundTexture;
    }

    public void DrawBackground()
    {
        _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, GameSettings.ScreenWidth, GameSettings.ScreenHeight), Color.White);
    }

}
