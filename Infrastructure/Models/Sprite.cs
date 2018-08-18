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
    public class Sprite : DrawableGameComponent
    {
        public event EventHandler<ScoreChangedEventArgs> ScoreChanged;

        public event EventHandler<EventArgs> GameOver;

        public event EventHandler<EventArgs> LevelPassed;

        public event EventHandler<GameManagerEventArgs> PlayerDown;

        public event EventHandler<EventArgs> RemoveMeAsNotifier;

        public event EventHandler<EventArgs> Disposed;

        public event EventHandler<EventArgs> PositionChanged;

        public event EventHandler<EventArgs> SizeChanged;

        private string m_AssetName;
        private float m_ScalingTimeToZero;
        private float m_CyclesPerSecond;
        private float m_BlinkingPerSecond;
        private float m_TimeToFade;
        protected Texture2D m_Texture;
        private Color[] m_TexturePixels;
        private List<Point> m_CollidedPixels;
        protected Vector2 m_Position;
        protected Color m_TintColor = Color.White;
        protected Vector2 m_Velocity = Vector2.Zero;
        protected SpriteEffects m_SpriteEffects;
        private SpriteBatch m_SpriteBatch;
        protected float m_WidthBeforeScale;
        protected float m_HeightBeforeScale;
        protected eTeam m_Team;
        private bool m_IsOutOfScreen;
        protected Vector2 m_Scales;
        private bool m_UseSharedBatch;
        private Rectangle m_SourceRectangle;
        private Vector2 m_PositionOrigin;
        private Vector2 m_RotationOrigin;
        private float m_AngularVelocity;
        private float m_Rotation;
        protected float m_LayerDepth;
        protected bool m_IsDying;
        private double m_TimeToNextShow;
        protected Vector2 m_Direction;
        private bool m_PulseShrinking;
        private float m_PulseTargetScale;
        private float m_PulseSourceScale;        
        private float m_PulseDeltaScale;
        private bool m_IsPulseAnimationInitialized;

        protected bool PulseShrinking
        {
            get { return m_PulseShrinking; }
            set { m_PulseShrinking = value; }
        }

        protected float PulseTargetScale
        {
            get { return m_PulseTargetScale; }
            set { m_PulseTargetScale = value; }
        }

        protected float PulseSourceScale
        {
            get { return m_PulseSourceScale; }
            set { m_PulseSourceScale = value; }
        }

        protected float PulseDeltaScale
        {
            get { return m_PulseDeltaScale; }
            set { m_PulseDeltaScale = value; }
        }

        protected bool UseSharedBatch
        {
            get { return m_UseSharedBatch; }
            set { m_UseSharedBatch = value; }
        }

        public List<Point> CollidedPixels
        {
            get { return m_CollidedPixels; }
            set { m_CollidedPixels = value; }
        }
        
        public Color[] TexturePixels
        {
            get { return m_TexturePixels; }
            set { m_TexturePixels = value; }
        }

        public bool IsDying
        {
            get { return m_IsDying; }
            set { m_IsDying = value; }
        }

        protected float ScalingTimeToZero
        {
            get { return m_ScalingTimeToZero; }
            set { m_ScalingTimeToZero = value; }
        }

        protected float CyclesPerSecond
        {
            get { return m_CyclesPerSecond; }
            set { m_CyclesPerSecond = value; }
        }

        public float BlinkingPerSecond
        {
            get { return m_BlinkingPerSecond; }
            set { m_BlinkingPerSecond = value; }
        }

        public float TimeToFade
        {
            get { return m_TimeToFade; }
            set { m_TimeToFade = value; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
            set 
            { 
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        public eTeam Team
        {
            get { return m_Team; }
            set { m_Team = value; }
        }

        public Rectangle Boundry
        {
            get
            {
                return new Rectangle(
                  (int)TopLeftPosition.X,
                  (int)TopLeftPosition.Y,
                  (int)this.Width,
                  (int)this.Height);
            }
        }

        public Rectangle BoundryBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        protected float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        protected float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        public bool IsOutOfScreen
        {
            get { return m_IsOutOfScreen; }
            set { m_IsOutOfScreen = value; }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
        }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }

        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        protected float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    OnPositionChanged();
                }
            }
        }

        protected Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(m_SourceRectangle.Width / 2), (float)(m_SourceRectangle.Height / 2)); }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }

        protected string AssetName
        {
            get { return m_AssetName; }
            set { m_AssetName = value; }
        }

        protected Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }
        
        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public float AngularVelocity
        {
            get { return m_AngularVelocity; }
            set { m_AngularVelocity = value; }
        }

        public Vector2 Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        protected Sprite(Game i_Game, string i_AssetName)
            : base(i_Game)
        {
            this.AssetName = i_AssetName;
            m_CollidedPixels = new List<Point>();
            m_ScalingTimeToZero = 0f;
            m_CyclesPerSecond = 0f;
            m_BlinkingPerSecond = 0f;
            m_UseSharedBatch = true;
            this.Visible = true;
            this.Direction = new Vector2(1, 0);
            m_IsOutOfScreen = false;
            m_Scales = Vector2.One;
            m_RotationOrigin = Vector2.Zero;
            m_SpriteEffects = SpriteEffects.None;
            m_AngularVelocity = 0;
            m_Rotation = 0;
            m_TimeToFade = 0;
            this.IsDying = false;
        }

        public override void Initialize()
        {
            ICollisionsManager collisionMgr;

            base.Initialize();
            InitPosition();
            if (this is ICollidable)
            {
                collisionMgr = this.Game.Services.GetService(typeof(ICollisionsManager)) as ICollisionsManager;
                if (collisionMgr != null)
                {
                    collisionMgr.AddObjectToMonitor(this as ICollidable);
                }
            }
        }

        protected virtual void LoadObject()
        {
            this.Texture = Game.Content.Load<Texture2D>(this.AssetName);
        }

        protected override void LoadContent()
        {
            LoadObject();
            if (m_SpriteBatch == null)
            {
                m_SpriteBatch = this.Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }

            if (this is ICollidable)
            {
                this.TexturePixels = new Color[this.Texture.Width * this.Texture.Height];
                this.Texture.GetData<Color>(this.TexturePixels);
            }

            base.LoadContent();
        }

        public override void Update(GameTime i_GameTime)
        {
            float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            this.Position += this.Velocity * totalSeconds;
            this.Rotation += this.AngularVelocity * totalSeconds;
            if (this.IsDying)
            {
                DyingAnimation(i_GameTime);
            }

            base.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (Visible)
            {
                if (!m_UseSharedBatch)
                {
                    m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                }

                m_SpriteBatch.Draw(
                    this.Texture, 
                    this.PositionForDraw, 
                    this.SourceRectangle,
                    this.TintColor, 
                    this.Rotation, 
                    this.RotationOrigin, 
                    this.Scales, 
                    this.SpriteEffects, 
                    this.LayerDepth);
                if (!m_UseSharedBatch)
                {
                    m_SpriteBatch.End();
                }
            }

            base.Draw(i_GameTime);
        }
        
        protected virtual void InitPosition()
        {
            m_HeightBeforeScale = Texture.Height;
            m_WidthBeforeScale = Texture.Width;
            InitSourceRectangle();
            InitOrigins();
        }

        protected virtual void InitOrigins()
        {
        }

        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        protected virtual void DyingAnimation(GameTime i_GameTime)
        { 
        }

        public virtual void ScalingDownAnimation(GameTime i_GameTime, float i_ScalingTimeToZero)
        {
            this.Scales -= new Vector2((1 / i_ScalingTimeToZero) * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
        }

        public virtual void RotatingAnimation(float i_CyclesPerSecond)
        {
            this.AngularVelocity = i_CyclesPerSecond * MathHelper.TwoPi;
        }

        public virtual void BlinkingAnimation(GameTime i_GameTime, float i_BlinkingPerSecond)
        {
            m_TimeToNextShow += i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimeToNextShow >= 1 / i_BlinkingPerSecond)
            {
                Visible = !Visible;
                m_TimeToNextShow -= 1 / i_BlinkingPerSecond;
            }
        }

        public virtual void FadeAnimation(GameTime i_GameTime, float i_TimeToFade)
        {
            m_TimeToNextShow += i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimeToNextShow <= i_TimeToFade)
            {
                if (this.Opacity != 0)
                {
                    this.Opacity -= 1 / i_TimeToFade * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    this.Visible = false;
                }
            }
        }

        private void initializePulseAnimation(float i_TargetScale)
        {
            if(!m_IsPulseAnimationInitialized)
            {
                this.PulseTargetScale = i_TargetScale; 
                this.PulseSourceScale = this.Scales.X;
                this.PulseDeltaScale = this.PulseTargetScale - this.PulseSourceScale;
                this.PulseShrinking = this.PulseDeltaScale < 0;
                m_IsPulseAnimationInitialized = true;
            }
        }

        public virtual void PulseAnimation(GameTime i_GameTime, float i_TargetScale, float i_PulsePerSecond)
        {
            float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            initializePulseAnimation(i_TargetScale);
            if (this.PulseShrinking)
            {
                if (this.Scales.X > this.PulseTargetScale)
                {
                    this.Scales -= new Vector2(totalSeconds * 2 * i_PulsePerSecond * this.PulseDeltaScale);
                }
                else
                {
                    this.Scales = new Vector2(this.PulseTargetScale);
                    this.PulseShrinking = false;
                    this.PulseTargetScale = this.PulseSourceScale;
                    this.PulseSourceScale = this.Scales.X;
                }
            }
            else
            {
                if (this.Scales.X < this.PulseTargetScale)
                {
                    this.Scales += new Vector2(totalSeconds * 2 * i_PulsePerSecond * this.PulseDeltaScale);
                }
                else
                {
                    this.Scales = new Vector2(this.PulseTargetScale);
                    this.PulseShrinking = true;
                    this.PulseTargetScale = this.PulseSourceScale;
                    this.PulseSourceScale = this.Scales.X;
                }
            }
        }

        public void NotifyGameManagerAboutMe()
        {
            IGameManager gameManager;

            if (this is IUpdateGameManager)
            {
                gameManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;
                if (gameManager != null)
                {
                    gameManager.AddObjectToMonitor(this as IUpdateGameManager);
                }
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

        protected virtual void OnPlayerDown(GameManagerEventArgs e)
        {
            if (PlayerDown != null)
            {
                PlayerDown.Invoke(this, e);
            }
        }

        protected internal void OnRemoveMeAsNotifier()
        {
            if(RemoveMeAsNotifier != null)
            {
                RemoveMeAsNotifier.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnLevelPassed()
        {
            if (LevelPassed != null)
            {
                LevelPassed.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnDisposed(object sender, EventArgs args)
        {
            if (Disposed != null)
            {
                Disposed.Invoke(sender, args);
            }
        }

        protected override void Dispose(bool i_Disposing)
        {
            base.Dispose(i_Disposing);
            OnDisposed(this, EventArgs.Empty);
        }

        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, EventArgs.Empty);
            }
        }

        protected virtual void OnSizeChanged()
        {
            if (SizeChanged != null)
            {
                SizeChanged(this, EventArgs.Empty);
            }
        }

        protected virtual bool CheckPixelCollision(Rectangle i_SourceBoundry, Color[] i_SourcePixels)
        {
            bool didCollide;
            int top;
            int bottom;
            int left;
            int right;
            Color color;
            Color sourceColor;
            Point collidedPixel;

            didCollide = false;
            top = Math.Max(this.Boundry.Top, i_SourceBoundry.Top);
            bottom = Math.Min(this.Boundry.Bottom, i_SourceBoundry.Bottom);
            left = Math.Max(this.Boundry.Left, i_SourceBoundry.Left);
            right = Math.Min(this.Boundry.Right, i_SourceBoundry.Right);
            for (int y = top; y < bottom; y++)
            {
                for(int x = left; x < right; x++)
                {
                    color = this.TexturePixels[(x - this.Boundry.Left) + ((y - this.Boundry.Top) * this.Boundry.Width)];
                    sourceColor = i_SourcePixels[(x - i_SourceBoundry.Left) + ((y - i_SourceBoundry.Top) * i_SourceBoundry.Width)];
                    if(color.A != 0 && sourceColor.A != 0)
                    {
                        didCollide = true;
                        collidedPixel = new Point(x, y);
                        m_CollidedPixels.Add(collidedPixel);
                    }
                }
            }

            return didCollide;
        }

        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;

            if (source != null)
            {
                if(source.Boundry.Intersects(this.Boundry))
                {
                    collided = CheckPixelCollision(source.Boundry, source.TexturePixels);
                }
            }

            return collided;
        }

        public virtual void Collided(ICollidable i_Collidable)
        {
            Sprite asSprite = i_Collidable as Sprite;

            if (Team != asSprite.Team)
            {
                this.Visible = false;
                this.Dispose();
            }
        }
    }
}