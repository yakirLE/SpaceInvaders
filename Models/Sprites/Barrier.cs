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
    public class Barrier : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"GameAssets\Barrier_44x32";
        private const float k_Speed = 40f;
        private const float k_SpeedUp = 1.06f;
        private const float k_PercentageToBiteTexture = 0.4f;
        private int m_Index;
        private Vector2 m_InitialPosition;
        private double m_TimeSpentBeforeChangingDirection;
        private ISoundManager m_SoundManager;

        public Vector2 InitialPosition
        {
            get { return m_InitialPosition; }
            set { m_InitialPosition = value; }
        }

        public int Index
        {
            get { return m_Index; }
            set { m_Index = value; }
        }

        public Barrier(Game i_Game, Color i_TintColor)
            : base(i_Game, k_AssetName)
        {
            m_SoundManager = this.Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            this.Direction = new Vector2(1, 0);
            this.TintColor = i_TintColor;
            calculateVelocityPerLevel();
        }

        private void calculateVelocityPerLevel()
        {
            int moduloResult;
            int currentInnerLevelPerRotation;
            IGameManager gameManager;

            gameManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;
            moduloResult = gameManager.CurrentLevel % gameManager.LevelRotation;
            currentInnerLevelPerRotation = moduloResult;
            if(moduloResult == 1)
            {
                this.Velocity = Vector2.Zero;
            }
            else if(moduloResult == 2)
            {
                this.Velocity = new Vector2(k_Speed, 0);
            }
            else
            {
                this.Velocity = new Vector2(k_Speed, 0);
                if(currentInnerLevelPerRotation == 0)
                {
                    currentInnerLevelPerRotation = gameManager.LevelRotation;
                }

                currentInnerLevelPerRotation -= 2;
                for (int i = 0; i < currentInnerLevelPerRotation; i++)
                {
                    this.Velocity *= k_SpeedUp;
                }
            }
        }

        protected override void LoadContent()
        {
            Texture2D newTexture;

            base.LoadContent();
            this.Texture = Game.Content.Load<Texture2D>(this.AssetName);
            newTexture = new Texture2D(this.GraphicsDevice, this.Texture.Width, this.Texture.Height);
            newTexture.SetData<Color>(this.TexturePixels);
            this.Texture = newTexture;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_TimeSpentBeforeChangingDirection += i_GameTime.ElapsedGameTime.TotalSeconds;
            if (Math.Abs(this.Velocity.X * (float)m_TimeSpentBeforeChangingDirection * (float)this.Direction.X) >= this.TextureCenter.X)
            {
                m_TimeSpentBeforeChangingDirection = 0;
                this.Direction *= -1;
                this.Velocity *= this.Direction;
            }

            base.Update(i_GameTime);
        }

        private void biteTexture(int i_StartingX, int i_StartingY, Sprite i_Sprite)
        {
            int xBound;
            int yBound;

            m_SoundManager.PlayCue("BarrierHit");
            if (i_Sprite.Direction.Y == -1)
            {
                xBound = MathHelper.Clamp((int)i_Sprite.Width + i_StartingX, this.Boundry.Left, this.Boundry.Right);
                yBound = MathHelper.Clamp(i_StartingY - (int)(i_Sprite.Height * k_PercentageToBiteTexture), this.Boundry.Top, this.Boundry.Bottom);
                for (int i = i_StartingX; i < xBound; i++)
                {
                    for (int j = i_StartingY; j >= yBound; j--)
                    {
                        this.TexturePixels[(i - this.Boundry.Left) + ((j - this.Boundry.Top) * this.Boundry.Width)].A = 0;
                    }
                }
            }
            else
            {
                xBound = MathHelper.Clamp((int)i_Sprite.Width + i_StartingX, this.Boundry.Left, this.Boundry.Right);
                yBound = MathHelper.Clamp(i_StartingY + (int)(i_Sprite.Height * k_PercentageToBiteTexture), this.Boundry.Top, this.Boundry.Bottom);
                for (int i = i_StartingX; i < xBound; i++)
                {
                    for (int j = i_StartingY; j < yBound; j++)
                    {
                        this.TexturePixels[(i - this.Boundry.Left) + ((j - this.Boundry.Top) * this.Boundry.Width)].A = 0;
                    }
                }
            }
        }

        private void eraseTexture(Point i_Point, Sprite i_EraserTexture)
        {
            int top;
            int bottom;
            int left;
            int right;

            top = Math.Max(this.Boundry.Top, i_EraserTexture.Boundry.Top);
            bottom = Math.Min(this.Boundry.Bottom, i_EraserTexture.Boundry.Bottom);
            left = Math.Max(this.Boundry.Left, i_EraserTexture.Boundry.Left);
            right = Math.Min(this.Boundry.Right, i_EraserTexture.Boundry.Right);
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    this.TexturePixels[(x - this.Boundry.Left) + ((y - this.Boundry.Top) * this.Boundry.Width)].A = 0;
                }
            }

            this.Texture.SetData<Color>(this.TexturePixels);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            Sprite asSprite = i_Collidable as Sprite;
            Bullet asBullet;
            EnemySpaceShip asEnemySpaceShip;
            int xMin = this.Game.GraphicsDevice.Viewport.Width;
            int yMin = this.Game.GraphicsDevice.Viewport.Height;
            int yMax = -1;
            bool didGetHitByBullet = false;
            Point pointOfCollision = new Point();

            if (Team != asSprite.Team)
            {
                asBullet = asSprite as Bullet;
                asEnemySpaceShip = asSprite as EnemySpaceShip;
                foreach (Point collidedPixel in this.CollidedPixels)
                {
                    if (asBullet != null)
                    {
                        didGetHitByBullet = true;
                        xMin = Math.Min(xMin, collidedPixel.X);
                        if(asSprite.Direction.Y == 1)
                        {
                            yMin = Math.Min(yMin, collidedPixel.Y);
                        }
                        else
                        {
                            yMax = Math.Max(yMax, collidedPixel.Y);
                        }
                    }
                    else if(asEnemySpaceShip != null)
                    {
                        pointOfCollision = collidedPixel;
                        break;
                    }
                }

                if(didGetHitByBullet)
                {
                    if (asSprite.Direction.Y == 1)
                    {
                        biteTexture(xMin, yMin, asSprite);
                    }
                    else
                    {
                        biteTexture(xMin, yMax, asSprite);
                    }
                }
                else if(this.CollidedPixels.Count != 0)
                {
                    eraseTexture(pointOfCollision, asSprite);
                }

                this.CollidedPixels.Clear();
                this.Texture.SetData<Color>(this.TexturePixels);
            }
        }
    }
}
