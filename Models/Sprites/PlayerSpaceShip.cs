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
    public class PlayerSpaceShip : Sprite, ICollidable2D, IUpdateGameManager
    {
        private const string k_AssetName = @"GameAssets\Ship01_32x32";
        private const float k_Velocity = 140f;
        private const float k_TimeToAnimate = 2.6f;
        private IInputManager m_InputManager;
        private PlayerGun m_PlayerGun;
        private float m_FirstMouseX;
        private bool m_IsFirstMousePosition;
        private bool m_DidStrike;
        private double m_CurrentBlinkingTime;
        private bool m_DidFinishDying;
        private ePlayerType m_PlayerType;
        private Keys m_MoveLeft;
        private Keys m_MoveRight;
        private Keys m_Shoot;
        public static float m_PlayerSpaceShipHeight;

        public PlayerGun PlayerGun
        {
            get { return m_PlayerGun; }
            set { m_PlayerGun = value; }
        }

        public ePlayerType PlayerType
        {
            get { return m_PlayerType; }
            set { m_PlayerType = value; }
        }

        public float Speed
        {
            get { return k_Velocity; }
        }

        public bool DidFinishDying
        {
            get { return m_DidFinishDying; }
        }

        public bool DidStrike
        {
            get { return m_DidStrike; }
            set { m_DidStrike = value; }
        }
        
        public PlayerSpaceShip(Game i_Game, Color i_TintColor, ePlayerType i_PlayerType)
            : base(i_Game, k_AssetName)
        {
            AssetName = k_AssetName;
            TintColor = i_TintColor;
            m_PlayerType = i_PlayerType;
            m_InputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            m_PlayerGun = new PlayerGun(i_Game, this);
            Team = eTeam.Player;
            m_IsFirstMousePosition = true;
            m_DidStrike = false;
            m_DidFinishDying = false;
            this.TimeToFade = 2.6f;
            this.BlinkingPerSecond = 9;
            this.CyclesPerSecond = 3;
            setKeys();
            NotifyGameManagerAboutMe();
        }

        private void setKeys()
        {
            switch(m_PlayerType)
            {
                case ePlayerType.First:
                    m_MoveLeft = Keys.Left;
                    m_MoveRight = Keys.Right;
                    m_Shoot = Keys.Space;
                    break;
                case ePlayerType.Second:
                    m_MoveLeft = Keys.S;
                    m_MoveRight = Keys.F;
                    m_Shoot = Keys.E;
                    break;
            }
        }

        protected override void InitPosition()
        {
            float x;
            float y;

            base.InitPosition();
            m_PlayerSpaceShipHeight = this.Height;
            x = (int)m_PlayerType * this.Width;
            y = (float)GraphicsDevice.Viewport.Height;
            y -= this.Height;

            this.Position = new Vector2(x, y);
        }

        protected override void InitOrigins()
        {
            base.InitOrigins();
            this.RotationOrigin = this.SourceRectangleCenter;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_PlayerGun.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            float xMousePosition;

            if (!this.IsDying)
            {
                if (m_InputManager.IsMouseMoving() && m_PlayerType == ePlayerType.First)
                {
                    improveMousePosition();
                    xMousePosition = m_InputManager.MouseState.Position.X - m_FirstMouseX;
                    this.Position = new Vector2(xMousePosition, Position.Y);
                }
                else if (m_InputManager.IsKeyHeld(m_MoveLeft))
                {
                    this.Direction = new Vector2(-1, 0);
                    Velocity = k_Velocity * this.Direction;
                }
                else if (m_InputManager.IsKeyHeld(m_MoveRight))
                {
                    this.Direction = new Vector2(1, 0);
                    Velocity = k_Velocity * this.Direction;
                }
                else
                {
                    Velocity = Vector2.Zero;
                }
            }
            else
            {
                this.Velocity = Vector2.Zero;
            }

            this.Position = new Vector2(MathHelper.Clamp(this.Position.X, 0, this.Game.GraphicsDevice.Viewport.Width - this.Width), Position.Y);
            if ((m_InputManager.IsLeftButtonPressed() && m_PlayerType == ePlayerType.First) || m_InputManager.IsKeyPressed(m_Shoot))
            {
                m_PlayerGun.Shoot();
            }

            if(this.DidStrike)
            {
                this.strikeAnimation(i_GameTime);
            }

            base.Update(i_GameTime);
        }

        private void improveMousePosition()
        {   
            if (m_IsFirstMousePosition && m_InputManager.MouseState.Position.X > 0)
            {
                m_FirstMouseX = m_InputManager.MouseState.Position.X;
                m_IsFirstMousePosition = false;
            }
            else if (m_InputManager.MouseState.Position.X <= 0)
            {
                m_FirstMouseX = 0;
            }
        }

        public void RemoveBulletFromCollection(Bullet i_Bullet)
        {
            m_PlayerGun.Bullets.Remove(i_Bullet);
        }

        private double getAnimationTime(GameTime i_GameTime)
        {
            m_CurrentBlinkingTime += i_GameTime.ElapsedGameTime.TotalSeconds;

            return m_CurrentBlinkingTime;
        }

        private void strikeAnimation(GameTime i_GameTime)
        {
            if (getAnimationTime(i_GameTime) <= k_TimeToAnimate)
            {
                this.BlinkingAnimation(i_GameTime, this.BlinkingPerSecond);
            }
            else
            {
                this.DidStrike = false;
                m_CurrentBlinkingTime = 0;
                this.Position = new Vector2((int)m_PlayerType * this.Width, this.Position.Y);
                this.Visible = true;
            }
        }

        protected override void DyingAnimation(GameTime i_GameTime)
        {
            GameManagerEventArgs gameManagerEventArgs;

            if (getAnimationTime(i_GameTime) <= k_TimeToAnimate)
            {
                this.Dispose();
                this.RotatingAnimation(this.CyclesPerSecond);
                this.FadeAnimation(i_GameTime, this.TimeToFade);
            }
            else
            {
                m_DidFinishDying = true;
                gameManagerEventArgs = new GameManagerEventArgs();
                gameManagerEventArgs.PlayerType = this.PlayerType;
                OnPlayerDown(gameManagerEventArgs);
                OnRemoveMeAsNotifier();
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            Bullet asBullet = i_Collidable as Bullet;
            ScoreChangedEventArgs scoreChangedEventArgs;

            if (asBullet != null && Team != asBullet.Team)
            {
                scoreChangedEventArgs = new ScoreChangedEventArgs();
                scoreChangedEventArgs.DidHitEnemy = false;
                scoreChangedEventArgs.PlayerType = this.PlayerType;
                OnScoreChanged(scoreChangedEventArgs);
            }
        }
    }
}