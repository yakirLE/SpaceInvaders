using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Managers
{
    public class DummyInputManager : IInputManager
    {
        private MouseState m_MouseState;
        private KeyboardState m_KeyboardState;

        public MouseState MouseState
        {
            get { return m_MouseState; }
        }

        public KeyboardState KeyboardState
        {
            get { return m_KeyboardState; }
        }

        public DummyInputManager()
        {
            m_MouseState = new MouseState();
            m_KeyboardState = new KeyboardState();
        }

        public bool IsLeftButtonPressed()
        {
            return false;
        }

        public bool IsKeyPressed(Keys i_Key)
        {
            return false;
        }

        public bool IsKeyHeld(Keys i_Key)
        {
            return false;
        }

        public bool IsKeyReleased(Keys i_Key)
        {
            return false;
        }

        public bool IsMouseMoving()
        {
            return false;
        }
    }
}
