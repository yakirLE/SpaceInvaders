using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Enums;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class Player : CompositeDrawableComponent<DrawableGameComponent>, IPlayer
    {
        private int m_Score;
        private Color m_TintColor;
        protected PlayerSpaceShip m_PlayerSpaceShip;
        private ePlayerType m_PlayerType;
        private Stack<PlayerSouls> m_Souls;
        private PlayerText m_Text;
        private bool m_IsDead;
        private ISoundManager m_SoundManager;

        public PlayerSpaceShip PlayerSpaceShip
        {
            get { return m_PlayerSpaceShip; }
            set { m_PlayerSpaceShip = value; }
        }

        public bool IsDead
        {
            get { return m_IsDead; }
            set { m_IsDead = value; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }
        
        public bool DidFinishDying
        {
            get { return m_PlayerSpaceShip.DidFinishDying; }
        }

        public ePlayerType PlayerType
        {
            get { return m_PlayerType; }
            set { m_PlayerType = value; }
        }
             
        public int Souls
        {
            get { return m_Souls.Count; }
        }

        public Player(Game i_Game, Color i_TintColor, ePlayerType i_PlayerType)
            : base(i_Game)
        {
            m_SoundManager = this.Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            m_Score = 0;
            m_TintColor = i_TintColor;
            m_PlayerType = i_PlayerType;
            m_PlayerSpaceShip = new PlayerSpaceShip(Game, m_TintColor, m_PlayerType);
            this.Add(m_PlayerSpaceShip);
            addSoulsToCollection();
            m_Text = new PlayerText(Game, m_PlayerType, @"Fonts\Calibri", m_TintColor);
            this.Add(m_Text);
        }

        private void addSoulsToCollection()
        {
            m_Souls = new Stack<PlayerSouls>();
            m_Souls.Push(new PlayerSouls(Game, m_TintColor, m_PlayerType, 0));
            this.Add(m_Souls.Peek());
            m_Souls.Push(new PlayerSouls(Game, m_TintColor, m_PlayerType, 1));
            this.Add(m_Souls.Peek());
            m_Souls.Push(new PlayerSouls(Game, m_TintColor, m_PlayerType, 2));
            this.Add(m_Souls.Peek());
        }

        public void PlayerGotHitByBullet()
        {
            PlayerSouls TopSoul = m_Souls.Peek();

            if (this.Souls > 1)
            {
                m_SoundManager.PlayCue("LifeDie");
                this.m_PlayerSpaceShip.DidStrike = true;
            }
            else
            {
                this.m_PlayerSpaceShip.IsDying = true;
            }

            KillSoul(TopSoul);
            m_Score = Math.Max(0, m_Score - 1500);
        }

        private void KillSoul(PlayerSouls TopSoul)
        {
            TopSoul.Dispose();
            TopSoul.Visible = false;
            m_Souls.Pop();
        }

        public void EnemySpaceShipDestroyed(int i_EnemyPointsWorth)
        {
            m_Score += (int)i_EnemyPointsWorth;
        }

        public override void Initialize()
        {
            IGameManager playerManager;

            base.Initialize();
            playerManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;
            if(playerManager != null)
            {
                playerManager.AddObjectToNotify(this as IPlayer);
            }

            m_Text.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            m_Text.StringToPrint = string.Format("P{0} Score: {1}", ((int)this.PlayerType + 1).ToString(), this.Score.ToString());
            this.PlayerSpaceShip.PlayerGun.Update(i_GameTime);
            base.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            this.PlayerSpaceShip.PlayerGun.Draw(i_GameTime);
        }

        public void UpdateScore(ScoreChangedEventArgs args)
        {
            if(args.DidHitEnemy)
            {
                EnemySpaceShipDestroyed(args.EnemyPointsWorth);
            }
            else
            {
                PlayerGotHitByBullet();
            }
        }

        public void NotifyMe()
        {
            IGameManager gameManager;

            if(this is IPlayer)
            {
                gameManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;
                if(gameManager != null)
                {
                    gameManager.AddObjectToNotify(this as IPlayer);
                }
            }
        }
    }
}
