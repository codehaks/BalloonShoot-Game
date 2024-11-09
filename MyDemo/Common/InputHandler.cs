using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonShoot.Common;
public class InputHandler
{
    private bool _mouseReleased = true;

    public bool IsLeftMouseClick(MouseState mouseState)
    {
        if (mouseState.LeftButton == ButtonState.Pressed && _mouseReleased)
        {
            // Return true on first press without resetting _mouseReleased yet
            return true;
        }

        if (mouseState.LeftButton == ButtonState.Released)
            _mouseReleased = true;

        return false;
    }

}

