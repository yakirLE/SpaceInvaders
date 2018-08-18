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

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class EnemyMatrix : CompositeDrawableComponent<DrawableGameComponent>, IUpdateGameManager
    {
        public event EventHandler<GameManagerEventArgs> PlayerDown;
        
        public event EventHandler<ScoreChangedEventArgs> ScoreChanged;

        public event EventHandler<EventArgs> GameOver;

        public event EventHandler<EventArgs> LevelPassed;

        public event EventHandler<EventArgs> RemoveMeAsNotifier;

        private const int k_Cols = 9;
        private const int k_Rows = 5;
        private int m_AdditionalColsPerLevel;
        public static bool m_IsSpeedChange;
        private EnemySpaceShip[,] m_Matrix;
        private Color m_EnemySpaceShipColor;
        private eEnemyType m_EnemyType;
        private bool m_ShouldSwitchDirection;
        private float m_DistanceToFill;
        private int m_EnemiesDied;
        
        public int Cols
        {
            get { return k_Cols; }
        }

        public int Rows
        {
            get { return k_Rows; }
        }
        
        public EnemyMatrix(Game i_Game)
            : base(i_Game)
        {
            calculateAdditionalColumnsPerLevel(i_Game);
            m_Matrix = new EnemySpaceShip[k_Rows, k_Cols + m_AdditionalColsPerLevel];
            m_ShouldSwitchDirection = false;
            m_DistanceToFill = 0;
            m_IsSpeedChange = false;
            m_EnemiesDied = 0;
            NotifyGameManagerAboutMe();
        }

        private void calculateAdditionalColumnsPerLevel(Game i_Game)
        {
            IGameManager gameManager;

            gameManager = i_Game.Services.GetService(typeof(IGameManager)) as IGameManager;
            m_AdditionalColsPerLevel = gameManager.RemainderPlusLevelCycle;
        }

        private void fillMatrix()
        {
            for (int i = 0; i < k_Rows; i++)
            {
                setEnemySpaceShipProperties(i);
                for (int j = 0; j < k_Cols + m_AdditionalColsPerLevel; j++)
                {
                    m_Matrix[i, j] = new EnemySpaceShip(Game, m_EnemySpaceShipColor, m_EnemyType, new Point(i, j));
                    m_Matrix[i, j].EnemyDied += enemySpaceShip_EnemyDied;
                    this.Add(m_Matrix[i, j]);
                }
            }
        }

        private void setEnemySpaceShipProperties(int i_Index)
        {
            if (i_Index == 0)
            {
                m_EnemySpaceShipColor = Color.LightPink;
                m_EnemyType = eEnemyType.HardEnemy;
            }
            else if (i_Index == 1 || i_Index == 2)
            {
                m_EnemySpaceShipColor = Color.LightBlue;
                m_EnemyType = eEnemyType.MediumEnemy;
            }
            else
            {
                m_EnemySpaceShipColor = Color.LightYellow;
                m_EnemyType = eEnemyType.EasyEnemy;
            }
        }

        public override void Initialize()
        {
            fillMatrix();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach(EnemySpaceShip enemySpaceShip in m_Matrix)
            {
                if (enemySpaceShip.NextXJump + enemySpaceShip.Width >= Game.GraphicsDevice.Viewport.Width)
                {
                    m_DistanceToFill = Game.GraphicsDevice.Viewport.Width - enemySpaceShip.Position.X;
                }
                else if (enemySpaceShip.NextXJump <= 0)
                {
                    m_DistanceToFill = enemySpaceShip.Position.X * -1;
                }

                if ((didReachLeftBoundry(enemySpaceShip) || didReachRightBoundry(enemySpaceShip)) && enemySpaceShip.Visible)
                {
                    m_ShouldSwitchDirection = true;
                    break;
                }
            }

            if(m_ShouldSwitchDirection)
            {
                float nextXJump;

                foreach (EnemySpaceShip enemySpaceShip in m_Matrix)
                {
                    if(enemySpaceShip.Position.X != enemySpaceShip.PrevX)
                    {
                        if (m_DistanceToFill != 0)
                        {
                            nextXJump = enemySpaceShip.Position.X + (m_DistanceToFill * enemySpaceShip.Direction.X);
                        }
                        else
                        {
                            nextXJump = enemySpaceShip.Position.X;
                        }

                        enemySpaceShip.Position = new Vector2(nextXJump, enemySpaceShip.Position.Y + (enemySpaceShip.Width / 2));
                        enemySpaceShip.TimeBetweenJumps *= 0.94f;
                        enemySpaceShip.PrevX = enemySpaceShip.Position.X;
                    }
                    else
                    {
                        break;
                    }
                }

                m_DistanceToFill = 0;
                foreach (EnemySpaceShip enemySpaceShip in m_Matrix)
                {
                    enemySpaceShip.Direction = -enemySpaceShip.Direction;
                }

                m_ShouldSwitchDirection = false;
            }

            if (m_IsSpeedChange)
            {
                foreach (EnemySpaceShip enemy in this)
                {
                    enemy.TimeBetweenJumps *= 0.94f;
                }

                m_IsSpeedChange = false;
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            foreach(EnemySpaceShip enemySpaceShip in m_Matrix)
            {
                enemySpaceShip.EnemyGun.Draw(i_GameTime);
            }
        }

        private bool didReachRightBoundry(EnemySpaceShip i_Enemy)
        {
            return i_Enemy.Position.X + i_Enemy.Width >= Game.GraphicsDevice.Viewport.Bounds.Right;
        }

        private bool didReachLeftBoundry(EnemySpaceShip i_Enemy)
        {
            return i_Enemy.Position.X <= Game.GraphicsDevice.Viewport.Bounds.Left;
        }

        public void NotifyGameManagerAboutMe()
        {
            IGameManager gameManager;

            gameManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;
            if (gameManager != null)
            {
                gameManager.AddObjectToMonitor(this as IUpdateGameManager);
            }
        }

        protected virtual void OnScoreChanged(ScoreChangedEventArgs e)
        {
            if(ScoreChanged != null)
            {
                ScoreChanged.Invoke(this, e);
            }
        }

        protected virtual void OnGameOver()
        {
            if (GameOver != null)
            {
                GameOver.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnLevelPassed()
        {
            if (LevelPassed != null)
            {
                LevelPassed.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnPlayerDown(GameManagerEventArgs e)
        {
            if (PlayerDown != null)
            {
                PlayerDown.Invoke(this, e);
            }
        }

        private void enemySpaceShip_EnemyDied(object sender, EventArgs e)
        {
            if (++m_EnemiesDied == (k_Cols + m_AdditionalColsPerLevel) * k_Rows)
            {
                OnLevelPassed();
                OnRemoveMeAsNotifier();
            }
        }

        protected internal void OnRemoveMeAsNotifier()
        {
            if (RemoveMeAsNotifier != null)
            {
                RemoveMeAsNotifier.Invoke(this, EventArgs.Empty);
            }
        }
    }
}