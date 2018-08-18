using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces
{
    public interface IInputManager
    {
        MouseState MouseState { get; }

        KeyboardState KeyboardState { get; }

        bool IsLeftButtonPressed();

        bool IsKeyPressed(Keys i_Key);

        bool IsKeyHeld(Keys i_Key);

        bool IsKeyReleased(Keys i_Key);

        bool IsMouseMoving();
    }
}
