using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Screens
{
    public class WelcomeScreen : GameScreen
    {
        private const float k_ScaleDownText = 2.8f;
        private Text m_WelcomeText;
        private MenuScreen m_MenuScreen;

        public MenuScreen MenuScreen
        {
            get { return m_MenuScreen; }
            set { m_MenuScreen = value; }
        }

        public WelcomeScreen(Game i_Game)
            : base(i_Game)
        {
            ScreenBackground background = new ScreenBackground(i_Game, Color.DarkCyan, @"GameAssets\BG_Space01_1024x768");
            SpaceInvadersTitleSprite welcomeSprite = new SpaceInvadersTitleSprite(i_Game);

            m_WelcomeText = new Text(this.Game, Color.YellowGreen);
            m_WelcomeText.Scales *= 1 / k_ScaleDownText;
            this.Add(background);
            this.Add(welcomeSprite);
            this.Add(m_WelcomeText);
        }

        private void initText()
        {
            float x;
            float y;
            Viewport viewport;

            viewport = this.Game.GraphicsDevice.Viewport;
            x = viewport.Width / 12;
            y = viewport.Height / 1.7f;
            m_WelcomeText.StringToPrint = @"Press Enter to Play!
Press Esc to exit
Press H for Main Menu";
            m_WelcomeText.Position = new Vector2(x, y);
        }

        public override void Initialize()
        {
            base.Initialize();
            initText();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if(this.InputManager.IsKeyPressed(Keys.Enter))
            {
                this.ExitScreen();
                this.ScreensManager.Push(new PlayScreen(this.Game));
                this.ScreensManager.SetCurrentItem(new LevelTransitionScreen(this.Game));
            }
            else if(this.InputManager.IsKeyPressed(Keys.Escape))
            {
                this.Game.Exit();
            }
            else if (this.InputManager.IsKeyPressed(Keys.H))
            {
                this.ExitScreen();
                this.ScreensManager.SetCurrentItem(this.MenuScreen);
            }
        }
    }
}
