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
    public class LevelTransitionScreen : GameScreen
    {
        private const float k_ScaleDownText = 2.8f;
        private int m_SecondsToCount;
        private Text m_TransitionText;
        private Text m_SecondsLeftText;
        private double m_TimePassed;

        public LevelTransitionScreen(Game i_Game)
            : base(i_Game)
        {
            m_SecondsToCount = 3;
            m_TimePassed = 0;
            m_TransitionText = new Text(this.Game, Color.YellowGreen);
            initText(m_TransitionText);
            m_SecondsLeftText = new Text(this.Game, Color.YellowGreen);
            initText(m_SecondsLeftText);
            this.IsModal = true;
            this.IsOverlayed = true;
            this.UseGradientBackground = true;
            this.BlackTintAlpha = 0.85f;
            this.Add(m_TransitionText);
            this.Add(m_SecondsLeftText);
        }

        private void initText(Text i_Text)
        {
            i_Text.Scales *= 1 / k_ScaleDownText;
        }

        private void countSeconds(GameTime i_GameTime)
        {
            m_TimePassed += i_GameTime.ElapsedGameTime.TotalSeconds;
            if(m_TimePassed >= 1)
            {
                m_SecondsToCount--;
                m_TimePassed--;
            }
        }

        private string showSecondsLeft(GameTime i_GameTime)
        {
            countSeconds(i_GameTime);
            
            return string.Format("{0}", m_SecondsToCount);
        }

        private int getCurrentLevel()
        {
            IGameManager gameManager;

            gameManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;

            return gameManager.CurrentLevel;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_TransitionText.StringToPrint = string.Format("Level {0}", getCurrentLevel().ToString());
            m_TransitionText.PositionOrigin = m_TransitionText.FontCenter;
            m_TransitionText.Position = this.CenterOfViewPort;
            m_SecondsLeftText.StringToPrint = string.Format("{0}", showSecondsLeft(i_GameTime));
            m_SecondsLeftText.PositionOrigin = m_SecondsLeftText.FontCenter;
            m_SecondsLeftText.Position = new Vector2(m_TransitionText.Position.X, m_TransitionText.Position.Y + m_TransitionText.Height);
            if(m_SecondsToCount == 0)
            {
                ExitScreen();
            }
        }
    }
}
