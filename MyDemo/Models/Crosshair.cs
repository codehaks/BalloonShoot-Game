using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace BalloonShoot.Models;

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
        int centeredX = mouseState.X - _width / 2;
        int centeredY = mouseState.Y - _height / 2;
        spriteBatch.Draw(_texture, new Rectangle(centeredX, centeredY, _width, _height), Color.White);
    }
}

