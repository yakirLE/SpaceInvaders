using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;
using C16_Ex03_Yakir_201049475_Omer_300471430.Managers;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public abstract class GameScreen : CompositeDrawableComponent<IGameComponent>
    {
        public event EventHandler Closed;

        protected bool m_IsModal;
        protected bool m_IsOverlayed;
        protected GameScreen m_PreviousScreen;
        protected bool m_HasFocus;
        protected IStackMananger<GameScreen> m_ScreenManager;
        protected float m_BlackTintAlpha;
        protected bool m_UseGradientBackground;
        private IInputManager m_InputManager;
        private IInputManager m_DummyInputManager;
        private Texture2D m_GradientTexture;
        private Texture2D m_BlankTexture;

        public bool UseGradientBackground
        {
            get { return m_UseGradientBackground; }
            set { m_UseGradientBackground = value; }
        }

        public float BlackTintAlpha
        {
            get { return m_BlackTintAlpha; }
            set { m_BlackTintAlpha = value; }
        }

        public IStackMananger<GameScreen> ScreensManager
        {
            get { return m_ScreenManager; }
            set { m_ScreenManager = value; }
        }

        public IInputManager InputManager
        {
            get { return this.HasFocus ? m_InputManager : m_DummyInputManager; }
        }

        public bool HasFocus
        {
            get { return m_HasFocus; }
            set { m_HasFocus = value; }
        }

        public GameScreen PreviousScreen
        {
            get { return m_PreviousScreen; }
            set { m_PreviousScreen = value; }
        }

        public bool IsOverlayed
        {
            get { return m_IsOverlayed; }
            set { m_IsOverlayed = value; }
        }

        public bool IsModal
        {
            get { return m_IsModal; }
            set { m_IsModal = value; }
        }

        public GameScreen(Game i_Game)
            : base(i_Game)
        {
            this.Enabled = false;
            this.Visible = false;
            m_IsModal = true;
            m_DummyInputManager = new DummyInputManager();
            m_BlackTintAlpha = 0;
            m_UseGradientBackground = false;
        }

        public void Activate()
        {
            this.Enabled = this.Visible = this.HasFocus = true;
            OnActivated();
        }

        protected virtual void OnActivated()
        {
            if(this.PreviousScreen != null && this.HasFocus)
            {
                this.PreviousScreen.HasFocus = false;
            }
        }

        public void Deactivate()
        {
            this.Enabled = this.Visible = this.HasFocus = false;
        }

        protected void ExitScreen()
        {
            Deactivate();
            OnClosed();
        }

        protected virtual void OnClosed()
        {
            if(Closed != null)
            {
                Closed.Invoke(this, EventArgs.Empty);
            }
        }

        public override void Initialize()
        {
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            if(m_InputManager == null)
            {
                m_InputManager = m_DummyInputManager;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_GradientTexture = this.Game.Content.Load<Texture2D>(@"Screens\gradient");
            m_BlankTexture = this.Game.Content.Load<Texture2D>(@"Screens\blank");
        }

        public override void Update(GameTime i_GameTime)
        {
            if(this.PreviousScreen != null && !this.IsModal)
            {
                this.PreviousScreen.Update(i_GameTime);
            }

            base.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            if(this.PreviousScreen != null && IsOverlayed)
            {
                this.PreviousScreen.Draw(i_GameTime);
                drawFadedDarkCoverIfNeeded();
            }

            base.Draw(i_GameTime);
        }

        private void drawFadedDarkCoverIfNeeded()
        {
            if(this.m_BlackTintAlpha > 0 || this.UseGradientBackground)
            {
                drawFadedDarkCover((byte)(m_BlackTintAlpha * byte.MaxValue));
            }
        }

        private void drawFadedDarkCover(byte i_Alpha)
        {
            Viewport viewport;
            Texture2D background;

            viewport = this.GraphicsDevice.Viewport;
            background = UseGradientBackground ? m_GradientTexture : m_BlankTexture;
            this.SpriteBatch.Begin();
            this.SpriteBatch.Draw(
                background,
                new Rectangle(0, 0, viewport.Width, viewport.Height),
                new Color(0, 0, 0, i_Alpha));
            this.SpriteBatch.End();
        }
    }
}
