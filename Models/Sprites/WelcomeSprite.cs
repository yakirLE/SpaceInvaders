using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class SpaceInvadersTitleSprite : Sprite
    {
        private const string k_AssetName = @"GameAssets\Space_Invaders_Title";
        private const float k_MaxScaleSize = 1.25f;
        private const float k_PulsePerSecond = 0.7f;
        private const float k_Speed = 100f;
        private double m_TimeSpentBeforeChangingDirection;

        public SpaceInvadersTitleSprite(Game i_Game)
            : base(i_Game, k_AssetName)
        {
            m_TimeSpentBeforeChangingDirection = 0;
            this.Velocity = new Vector2(k_Speed, 0);
        }

        protected override void InitOrigins()
        {
            this.PositionOrigin = this.TextureCenter;
        }

        protected override void InitPosition()
        {
            int y;
            int x;
            Viewport viewport;

            base.InitPosition();
            viewport = this.Game.GraphicsDevice.Viewport;
            x = viewport.Width / 2;
            y = (viewport.Height / 2) - (viewport.Height / 4);
            this.Position = new Vector2(x, y);
        }

        private void moveTextureFromSideToSide(GameTime i_GameTime)
        {
            m_TimeSpentBeforeChangingDirection += i_GameTime.ElapsedGameTime.TotalSeconds;
            if (Math.Abs(this.Velocity.X * (float)m_TimeSpentBeforeChangingDirection * (float)this.Direction.X) >= this.TextureCenter.X / 2)
            {
                m_TimeSpentBeforeChangingDirection = 0;
                this.Direction *= -1;
                this.Velocity *= this.Direction;
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            this.PulseAnimation(i_GameTime, k_MaxScaleSize, k_PulsePerSecond);
            moveTextureFromSideToSide(i_GameTime);
        }
    }
}
