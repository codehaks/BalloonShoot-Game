using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BalloonShoot.Models;

public class GameScore
{
    private SpriteFont _font;
    private int _score;

    public GameScore(SpriteFont font)
    {
        _font = font;
        _score = 0;
    }

    public void Increase()
    {
        _score++;
    }

    public void Decrease()
    {
        _score--;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_font, $"Score: {_score}", new Vector2(0, 0), Color.White);
    }
}

