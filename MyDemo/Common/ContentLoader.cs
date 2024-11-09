using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonShoot.Common;
public class ContentLoader
{
    private readonly ContentManager _contentManager;

    public Texture2D BalloonTexture { get; private set; }
    public Texture2D PopTexture { get; private set; }
    public Texture2D CrosshairTexture { get; private set; }
    public Texture2D BackgroundTexture { get; private set; }
    public SpriteFont GameFont { get; private set; }

    public ContentLoader(ContentManager contentManager)
    {
        _contentManager = contentManager;
    }

    public void LoadAllContent()
    {
        BalloonTexture = _contentManager.Load<Texture2D>("assets/balloon");
        PopTexture = _contentManager.Load<Texture2D>("assets/pop");
        CrosshairTexture = _contentManager.Load<Texture2D>("assets/crosshair");
        BackgroundTexture = _contentManager.Load<Texture2D>("assets/bg");
        GameFont = _contentManager.Load<SpriteFont>("assets/myfont");
    }
}