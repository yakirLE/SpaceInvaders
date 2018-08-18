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
    public class GameOverScreen : GameScreen
    {
        private const float k_ScaleDownText = 3.8f;
        private Text m_GameStatsText;
        private Text m_GameOverInstructionsText;
        private MenuScreen m_MenuScreen;

        public MenuScreen MenuScreen
        {
            get { return m_MenuScreen; }
            set { m_MenuScreen = value; }
        }

        public Text GameStatsText
        {
            get { return m_GameStatsText; }
            set { m_GameStatsText = value; }
        }

        public GameOverScreen(Game i_Game)
            : base(i_Game)
        {
            ScreenBackground background = new ScreenBackground(i_Game, Color.DarkRed, @"GameAssets\BG_Space01_1024x768");
            GameOverSprite gameOverSprite = new GameOverSprite(i_Game);

            m_GameStatsText = new Text(i_Game, Color.YellowGreen);
            m_GameOverInstructionsText = new Text(i_Game, Color.YellowGreen);
            m_GameStatsText.Scales *= 1 / k_ScaleDownText;
            m_GameOverInstructionsText.Scales *= 1 / k_ScaleDownText;
            this.Add(background);
            this.Add(gameOverSprite);
            this.Add(m_GameStatsText);
            this.Add(m_GameOverInstructionsText);
        }

        public override void Update(GameTime i_GameTime)
        {
            IGameManager gameManager;
            PlayScreen playScreen;
            LevelTransitionScreen levelTransitionScreen;

            base.Update(i_GameTime);
            PositionInstructionsText();
            if (this.InputManager.IsKeyPressed(Keys.Escape))
            {
                this.Game.Exit();
            }
            else if (this.InputManager.IsKeyPressed(Keys.R))
            {
                gameManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;
                gameManager.WasGameOverInitiated = false;
                levelTransitionScreen = new LevelTransitionScreen(this.Game);
                playScreen = new PlayScreen(this.Game);
                this.ScreensManager.Push(playScreen);
                this.ScreensManager.SetCurrentItem(levelTransitionScreen);
            }
            else if (this.InputManager.IsKeyPressed(Keys.H))
            {
                this.ScreensManager.SetCurrentItem(this.MenuScreen);
            }
        }

        private void PositionInstructionsText()
        {
            float x;
            float y;
            Viewport viewport;

            viewport = this.Game.GraphicsDevice.Viewport;
            m_GameOverInstructionsText.StringToPrint = @"Press Esc to Exit
Press R to Restart
Press H for Main Menu";
            x = m_GameOverInstructionsText.Width / 2;
            y = m_GameOverInstructionsText.Height;
            m_GameOverInstructionsText.PositionOrigin = new Vector2(x, y);
            x = viewport.Width / 4;
            y = (viewport.Height / 2) + (viewport.Height / 2.3f);
            m_GameOverInstructionsText.Position = new Vector2(x, y);
        }
    }
}
