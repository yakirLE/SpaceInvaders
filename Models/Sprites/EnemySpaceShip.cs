using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;
using C16_Ex03_Yakir_201049475_Omer_300471430.Enums;
using C16_Ex03_Yakir_201049475_Omer_300471430.CustomEventArgs;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class EnemySpaceShip : Sprite, ICollidable2D, IUpdateGameManager
    {
        public event EventHandler<EventArgs> EnemyDied;

        private const string k_AssetName = @"GameAssets\Enemies_192x32";
        private const int k_NumOfFrames = 2;
        private const int k_NumOfTextureFrames = 6;
        private const int k_InitialHeightShipUnits = 3;
        private const float k_ShipWidthDistance = 1.6f;
        private eEnemyType m_EnemyType;
        private float m_TimeBetweenJumps;
        private Point m_Index;
        private bool m_IsEnemyVisible; 
        private double m_TimeToNextShow;
        private int m_PrevDirection;
        private float m_PrevX;
        private float m_NextXJump;
        private EnemyGun m_EnemyGun;
        private int m_StartingFrameX;
        private int m_EndingFrameX;
        private int m_CurrentFrameIndex;
        private bool m_DidNotifyMatrixIDied = false;
        private ISoundManager m_SoundManager;

        public int EnemyPointsWorth
        {
            get
            {
                IGameManager gameManager;

                gameManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;
                return (int)this.EnemyType + (gameManager.IncreasedPointsPerEnemy * gameManager.RemainderPlusLevelCycle);
            }
        }

        public EnemyGun EnemyGun
        {
            get { return m_EnemyGun; }
            set { m_EnemyGun = value; }
        }

        public eEnemyType EnemyType
        {
            get { return m_EnemyType; }
            set { m_EnemyType = value; }
        }

        public bool IsEnemyVisible
        {
            get { return m_IsEnemyVisible; }
        }

        public float NextXJump
        {
            get { return m_NextXJump; }
        }

        public float TimeBetweenJumps
        {
            get { return m_TimeBetweenJumps; }
            set { m_TimeBetweenJumps = value; }
        }

        public float PrevX
        {
            get { return m_PrevX; }
            set { m_PrevX = value; }
        }

        public EnemySpaceShip(Game i_Game, Color i_TintColor, eEnemyType i_EnemyType, Point i_Index)
            : base(i_Game, k_AssetName)
        {
            m_SoundManager = this.Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            AssetName = k_AssetName;
            TintColor = i_TintColor;
            m_Index = i_Index;
            m_TimeBetweenJumps = 0.25f;
            m_IsEnemyVisible = true;
            m_TimeToNextShow = 0;
            this.Direction = new Vector2(-1, 0);
            m_PrevDirection = -1;
            m_PrevX = 0;
            m_NextXJump = 0;
            Team = eTeam.Enemy;
            m_EnemyGun = new EnemyGun(i_Game, this);
            m_EnemyType = i_EnemyType;
            m_CurrentFrameIndex = 0;
            this.IsDying = false;
            this.ScalingTimeToZero = 1.8f;
            this.CyclesPerSecond = 7f;
            setEnemyFrames();
            NotifyGameManagerAboutMe();
        }

        private void setEnemyFrames()
        {
            switch (m_EnemyType)
            {
                case eEnemyType.HardEnemy:
                    m_StartingFrameX = 0;
                    m_EndingFrameX = 63;
                    break;
                case eEnemyType.MediumEnemy:
                    m_StartingFrameX = 64;
                    m_EndingFrameX = 127;
                    break;
                case eEnemyType.EasyEnemy:
                    m_StartingFrameX = 128;
                    m_EndingFrameX = 191;
                    break;
            }
        }

        public void RemoveBulletFromCollection(Bullet i_Bullet)
        {
            m_EnemyGun.Bullets.Remove(i_Bullet);
        }
        
        protected override void InitPosition()
        {
            base.InitPosition();
            this.Width = SourceRectangle.Width;
            Position = new Vector2(m_Index.Y * Width * k_ShipWidthDistance, (Height * k_InitialHeightShipUnits) + (m_Index.X * Height * k_ShipWidthDistance));
        }

        protected override void InitOrigins()
        {
            base.InitOrigins();
            this.RotationOrigin = this.SourceRectangleCenter;
        }

        protected override void InitSourceRectangle()
        {
            this.SourceRectangle = new Rectangle(m_StartingFrameX, 0, (m_EndingFrameX - m_StartingFrameX + 1) / k_NumOfFrames, (int)Height);
        }

        private void switchFrame()
        {
            m_CurrentFrameIndex++;
            m_CurrentFrameIndex %= k_NumOfFrames;
            this.SourceRectangle = new Rectangle(m_StartingFrameX + ((int)this.Width * m_CurrentFrameIndex), 0, (int)this.Width, (int)this.Height);
        }

        private void checkIfGameOverByEnemySpaceShip()
        {
            float spaceShipHeight;

            spaceShipHeight = PlayerSpaceShip.m_PlayerSpaceShipHeight;
            if (Position.Y + Height >= Game.GraphicsDevice.Viewport.Height - spaceShipHeight && Visible == true)
            {
                OnGameOver();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_EnemyGun.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            bool isDirectionChanged = m_PrevDirection != this.Direction.X;
            float nextXJump;

            m_EnemyGun.Update(i_GameTime);
            checkIfGameOverByEnemySpaceShip();
            m_TimeToNextShow += i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimeToNextShow >= m_TimeBetweenJumps)
            {
                m_IsEnemyVisible = !m_IsEnemyVisible;
                m_TimeToNextShow -= m_TimeBetweenJumps;
                if (!m_IsEnemyVisible || isDirectionChanged)
                {
                    nextXJump = Position.X - (this.Direction.X * WidthBeforeScale / 2);
                    if (!this.IsDying)
                    {
                        switchFrame();
                    }

                    Position = new Vector2(nextXJump, Position.Y);
                    m_NextXJump = nextXJump - (this.Direction.X * WidthBeforeScale / 2);
                }

                m_EnemyGun.GameTime = i_GameTime;
                m_EnemyGun.Shoot();
                m_PrevDirection = (int)this.Direction.X;
            }

            base.Update(i_GameTime);
        }

        protected override void DyingAnimation(GameTime i_GameTime)
        {
            this.ScalingDownAnimation(i_GameTime, this.ScalingTimeToZero);
            this.RotatingAnimation(this.CyclesPerSecond);
            if (this.Scales.X <= 0)
            {
                this.Visible = false;
                if (!m_DidNotifyMatrixIDied)
                {
                    OnEnemyDied();
                    m_DidNotifyMatrixIDied = true;
                    OnRemoveMeAsNotifier();
                }
            }
        }

        protected virtual void OnEnemyDied()
        {
            if (EnemyDied != null)
            {
                EnemyDied.Invoke(this, EventArgs.Empty);
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            PlayerSpaceShip asPlayerSpaceShip;

            if ((i_Collidable as Sprite).Team == eTeam.Player)
            {
                asPlayerSpaceShip = i_Collidable as PlayerSpaceShip;
                this.IsDying = true;
                m_SoundManager.PlayCue("EnemyKill");
                this.Dispose();
                EnemyMatrix.m_IsSpeedChange = true;

                if (asPlayerSpaceShip != null)
                {
                    OnGameOver();
                }
            }
        }
    }
}
