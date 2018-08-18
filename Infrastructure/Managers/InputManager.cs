using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class InputManager : GameComponent, IInputManager
    {
        private MouseState m_MouseState;
        private MouseState m_PrevMouseState;
        private KeyboardState m_KeyboardState;
        private KeyboardState m_PrevKeyboardState;

        public KeyboardState PrevKeyboardState
        {
            set { m_PrevKeyboardState = value; }
        }

        public KeyboardState KeyboardState
        {
            get { return m_KeyboardState; }
            set { m_KeyboardState = value; }
        }

        public MouseState PrevMouseState
        {
            set { m_PrevMouseState = value; }
        }

        public MouseState MouseState
        {
            get { return m_MouseState; }
            set { m_MouseState = value; }
        }

        public InputManager(Game i_Game)
            : base(i_Game)
        {
            Game.Services.AddService(typeof(IInputManager), this);
            this.Game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            m_PrevKeyboardState = Keyboard.GetState();
            m_KeyboardState = m_PrevKeyboardState;
            m_PrevMouseState = Mouse.GetState();
            m_MouseState = m_PrevMouseState;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_PrevKeyboardState = m_KeyboardState;
            m_KeyboardState = Keyboard.GetState();
            m_PrevMouseState = m_MouseState;
            m_MouseState = Mouse.GetState();
        }

        public bool IsLeftButtonPressed()
        {
            return m_MouseState.LeftButton == ButtonState.Pressed && m_PrevMouseState.LeftButton == ButtonState.Released;
        }

        public bool IsKeyPressed(Keys i_Key)
        {
            return m_KeyboardState.IsKeyDown(i_Key) && m_PrevKeyboardState.IsKeyUp(i_Key);
        }

        public bool IsKeyHeld(Keys i_Key)
        {
            return m_KeyboardState.IsKeyDown(i_Key) && m_PrevKeyboardState.IsKeyDown(i_Key);
        }

        public bool IsKeyReleased(Keys i_Key)
        {
            return m_KeyboardState.IsKeyUp(i_Key) && m_PrevKeyboardState.IsKeyDown(i_Key);
        }

        public bool IsMouseMoving()
        {
            return m_MouseState.X != m_PrevMouseState.X;
        }
    }
}
