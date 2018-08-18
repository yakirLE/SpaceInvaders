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
    public class MotherShip : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"GameAssets\MotherShip_32x120";
        private const float k_Velocity = 95f;
        private double m_TimeSinceLastShow;
        private bool m_IsTimestampSaved;
        private int m_RandomWaitTime;
        private eEnemyType m_EnemyType;
        private Random m_Random;
        private ISoundManager m_SoundManager;

        public eEnemyType EnemyType
        {
            get { return m_EnemyType; }
            set { m_EnemyType = value; }
        }

        public int EnemyPointsWorth
        {
            get
            {
                return (int)this.EnemyType;
            }
        }

        public MotherShip(Game i_Game, Color i_TintColor)
            : base(i_Game, k_AssetName)
        {
            m_SoundManager = this.Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            this.AssetName = k_AssetName;
            this.TintColor = i_TintColor;
            this.Velocity = new Vector2(k_Velocity * this.Direction.X, 0);
            m_TimeSinceLastShow = 0;
            m_IsTimestampSaved = false;
            m_RandomWaitTime = 0;
            m_Random = new Random();
            this.Team = eTeam.Enemy;
            m_EnemyType = eEnemyType.MotherShip;
            this.ScalingTimeToZero = 2.6f;
            this.BlinkingPerSecond = 10f;
        }

        protected override void InitPosition()
        {
            float x;
            float y;

            base.InitPosition();
            x = -this.Width;
            y = this.Height;
            x -= this.Width / 2;
            this.Position = new Vector2(x, y);
        }

        public override void Update(GameTime i_GameTime)
        {
            CollisionsManager cm;

            if (isShipOutOfScreen())
            {
                spawnShipRandomly(i_GameTime);
                this.IsDying = false;
                this.Visible = true;
                this.Scales = Vector2.One;
                cm = Game.Services.GetService(typeof(ICollisionsManager)) as CollisionsManager;
                cm.AddObjectToMonitor(this as ICollidable);
            }

            base.Update(i_GameTime);
        }

        protected override void InitOrigins()
        {
            base.InitOrigins();
            this.RotationOrigin = this.SourceRectangleCenter;
        }

        protected override void DyingAnimation(GameTime i_GameTime)
        {
            this.ScalingDownAnimation(i_GameTime, this.ScalingTimeToZero);
            this.BlinkingAnimation(i_GameTime, this.BlinkingPerSecond);
            if (this.Scales.X <= 0)
            {
                this.Visible = false;
            }
        }

        private bool isShipOutOfScreen()
        {
            return this.Position.X > this.Game.GraphicsDevice.Viewport.Width;
        }

        private void spawnShipRandomly(GameTime i_GameTime)
        {
            if (!m_IsTimestampSaved)
            {
                m_RandomWaitTime = m_Random.Next(5, 6);
                m_TimeSinceLastShow = i_GameTime.TotalGameTime.TotalSeconds;
                m_IsTimestampSaved = true;
            }

            if (i_GameTime.TotalGameTime.TotalSeconds - m_TimeSinceLastShow >= (double)m_RandomWaitTime)
            {
                this.Position = new Vector2(-this.Width, this.Height);
                m_IsTimestampSaved = false;
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            Sprite asSprite = i_Collidable as Sprite;

            if (Team != asSprite.Team)
            {
                this.IsDying = true;
                m_SoundManager.PlayCue("MotherShipKill");
            }
        }
    }
}
