using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;
using C16_Ex03_Yakir_201049475_Omer_300471430.CustomEventArgs;
using C16_Ex03_Yakir_201049475_Omer_300471430.Enums;
using C16_Ex03_Yakir_201049475_Omer_300471430.Screens;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class GameManager : GameComponent, IGameManager
    {
        public event EventHandler<EventArgs> LevelPassed;

        public event EventHandler<EventArgs> GameOver;

        protected readonly List<IPlayer> m_Players = new List<IPlayer>();
        protected readonly List<IUpdateGameManager> m_UpdatesGameManager = new List<IUpdateGameManager>();
        protected int m_CurrentLevel;
        protected int m_CurrentAlivePlayers;
        private bool m_WasGameOverInitiated;
        private int m_LevelCycles;
        private int m_IncreasedPointsPerEnemy;
        private int m_RemainderOfLevel;
        private int m_RemainderPlusLevelCycle;
        private int m_PlayersAmount;
        private ISoundManager m_SoundManager;

        public int PlayersAmount
        {
            get { return m_PlayersAmount; }
            set { m_PlayersAmount = value; }
        }

        public int RemainderPlusLevelCycle
        {
            get { return m_RemainderPlusLevelCycle; }
        }

        public int IncreasedPointsPerEnemy
        {
            get { return m_IncreasedPointsPerEnemy; }
            set { m_IncreasedPointsPerEnemy = value; }
        }

        public int LevelRotation
        {
            get { return m_LevelCycles; }
            set { m_LevelCycles = value; }
        }

        public bool WasGameOverInitiated
        {
            get { return m_WasGameOverInitiated; }
            set { m_WasGameOverInitiated = value; }
        }

        public int CurrentLevel
        {
            get { return m_CurrentLevel; }
            set { m_CurrentLevel = value; }
        }

        public GameManager(Game i_Game)
            : base(i_Game)
        {
            m_SoundManager = this.Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            m_LevelCycles = 6;
            m_IncreasedPointsPerEnemy = 50;
            m_CurrentLevel = 1;
            m_CurrentAlivePlayers = 0;
            m_WasGameOverInitiated = false;
            this.Game.Services.AddService(typeof(IGameManager), this);
        }

        public void AddObjectToMonitor(IUpdateGameManager i_UpdatesGameManager)
        {
            if (!m_UpdatesGameManager.Contains(i_UpdatesGameManager))
            {
                m_UpdatesGameManager.Add(i_UpdatesGameManager);
                i_UpdatesGameManager.ScoreChanged += updatesGameManager_ScoreChanged;
                i_UpdatesGameManager.GameOver += updatesGameManager_GameOver;
                i_UpdatesGameManager.PlayerDown += updatesGameManager_PlayerDown;
                i_UpdatesGameManager.RemoveMeAsNotifier += updatesGameManager_RemoveMeAsNotifier;
                i_UpdatesGameManager.LevelPassed += updatesGameManager_LevelPassed;
            }
        }

        public void AddObjectToNotify(IPlayer i_Player)
        {
            if(!this.m_Players.Contains(i_Player))
            {
                this.m_Players.Add(i_Player);
                m_CurrentAlivePlayers++;
            }
        }

        private void updatesGameManager_ScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            checkWhoShouldBeNotifed(sender, e);
        }

        private IPlayer getWinner()
        {
            int maxScore = -1;
            int drawScore = -1;
            IPlayer winner = null;

            foreach(IPlayer player in m_Players)
            {
                if(player.Score > maxScore)
                {
                    winner = player;
                    maxScore = player.Score;
                }
                else if(player.Score == maxScore)
                {
                    drawScore = maxScore;
                }
            }

            if(drawScore == maxScore)
            {
                winner = null;
            }

            return winner;
        }

        private void updatesGameManager_GameOver(object sender, EventArgs e)
        {
            handleGameOverSituation();
        }

        private void handleGameOverSituation()
        {
            float x;
            float y;
            StringBuilder gameOverStats;
            IPlayer winner = null;
            IStackMananger<GameScreen> screensManager;
            GameOverScreen gameOverScreen;
            Viewport viewport;

            if (!m_WasGameOverInitiated)
            {
                m_SoundManager.PlayCue("GameOver");
                m_WasGameOverInitiated = true;
                viewport = this.Game.GraphicsDevice.Viewport;
                x = viewport.Width / 2;
                y = viewport.Height / 2;
                gameOverStats = new StringBuilder();
                screensManager = this.Game.Services.GetService(typeof(IStackMananger<GameScreen>)) as IStackMananger<GameScreen>;
                foreach (IPlayer player in m_Players)
                {
                    gameOverStats.AppendLine(string.Format("P{0} Score: {1}", (int)player.PlayerType + 1, player.Score));
                }

                if (m_Players.Count > 1)
                {
                    y = (viewport.Height / 2) + (viewport.Height / 16);
                    winner = getWinner();
                    gameOverStats.AppendLine(string.Empty);
                    if (winner == null)
                    {
                        gameOverStats.AppendLine("There is a Draw!");
                    }
                    else
                    {
                        gameOverStats.AppendLine(string.Format("The Winner is Player {0}!", (int)winner.PlayerType + 1));
                    }
                }

                m_Players.Clear();
                m_UpdatesGameManager.Clear();
                m_CurrentLevel = 1;
                OnGameOver();
                gameOverScreen = screensManager.ActiveItem as GameOverScreen;
                gameOverScreen.GameStatsText.StringToPrint = gameOverStats.ToString();
                gameOverScreen.GameStatsText.PositionOrigin = gameOverScreen.GameStatsText.FontCenter;
                gameOverScreen.GameStatsText.Position = new Vector2(x, y);
            }
        }

        private void updatesGameManager_PlayerDown(object sender, GameManagerEventArgs e)
        {
            handleDeadPlayer(sender as IUpdateGameManager, e.PlayerType);
        }

        private void updatesGameManager_RemoveMeAsNotifier(object sender, EventArgs e)
        {
            handleRemoveMeAsNotifier(sender as IUpdateGameManager);
        }

        private void handleRemoveMeAsNotifier(IUpdateGameManager i_UpdatesGameManager)
        {
            if(m_UpdatesGameManager.Contains(i_UpdatesGameManager))
            {
                i_UpdatesGameManager.ScoreChanged -= updatesGameManager_ScoreChanged;
                i_UpdatesGameManager.GameOver -= updatesGameManager_GameOver;
                i_UpdatesGameManager.PlayerDown -= updatesGameManager_PlayerDown;
                i_UpdatesGameManager.RemoveMeAsNotifier -= updatesGameManager_RemoveMeAsNotifier;
                i_UpdatesGameManager.LevelPassed -= updatesGameManager_LevelPassed;
                m_UpdatesGameManager.Remove(i_UpdatesGameManager);
            }
        }

        private void handleDeadPlayer(IUpdateGameManager i_UpdatesGameManager, ePlayerType i_PlayerType)
        {
            IPlayer playerToRemove = null;

            i_UpdatesGameManager.ScoreChanged -= updatesGameManager_ScoreChanged;
            i_UpdatesGameManager.GameOver -= updatesGameManager_GameOver;
            i_UpdatesGameManager.PlayerDown -= updatesGameManager_PlayerDown;
            if (m_CurrentAlivePlayers == 1)
            {
                m_Players[0].IsDead = true;
                handleGameOverSituation();
            }
            
            foreach (IPlayer player in m_Players)
            {
                if (player.PlayerType == i_PlayerType)
                {
                    playerToRemove = player;
                    player.IsDead = true;
                    break;
                }
            }

            m_CurrentAlivePlayers--;
        }

        private void checkWhoShouldBeNotifed(object sender, ScoreChangedEventArgs e)
        {
            foreach(IPlayer player in m_Players)
            {
                if (player.PlayerType == e.PlayerType)
                {
                    player.UpdateScore(e);
                    break;
                }
            }
        }

        protected virtual void OnLevelPassed()
        {
            if(LevelPassed != null)
            {
                LevelPassed.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnGameOver()
        {
            if (GameOver != null)
            {
                GameOver.Invoke(this, EventArgs.Empty);
            }
        }

        private void updatesGameManager_LevelPassed(object sender, EventArgs e)
        {
            m_SoundManager.PlayCue("LevelWin");
            m_CurrentLevel++;
            m_RemainderOfLevel = this.CurrentLevel % this.LevelRotation == 0 ? 0 : (this.CurrentLevel % this.LevelRotation) - 1;
            m_RemainderPlusLevelCycle = m_RemainderOfLevel + (this.CurrentLevel / this.LevelRotation * (this.LevelRotation - 1));
            m_UpdatesGameManager.Clear();
            OnLevelPassed();
        }
    }
}
