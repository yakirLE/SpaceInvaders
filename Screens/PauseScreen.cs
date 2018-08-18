using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Screens
{
    public class PauseScreen : GameScreen
    {
        private const float k_ScaleDownText = 4f;
        private Text m_PauseText;

        public PauseScreen(Game i_Game)
            : base(i_Game)
        {
            this.IsModal = true;
            this.IsOverlayed = true;
            this.UseGradientBackground = true;
            this.BlackTintAlpha = 0.60f;
            m_PauseText = new Text(i_Game, Color.YellowGreen);
            m_PauseText.Scales *= 1 / k_ScaleDownText;
            this.Add(m_PauseText);
        }

        private void initText(Text i_Text)
        {
            float x;
            float y;
            Viewport viewport;

            viewport = this.Game.GraphicsDevice.Viewport;
            x = viewport.Width / 2;
            y = viewport.Height / 12f;
            m_PauseText.StringToPrint = "   Game Paused.\nPress R to Resume";
            m_PauseText.PositionOrigin = m_PauseText.FontCenter;
            m_PauseText.Position = new Vector2(x, y);
        }

        public override void Initialize()
        {
            base.Initialize();
            initText(m_PauseText);
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (this.InputManager.IsKeyPressed(Keys.R))
            {
                ExitScreen();
            }
        }
    }
}
