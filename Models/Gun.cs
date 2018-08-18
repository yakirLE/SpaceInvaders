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
    public abstract class Gun : CompositeDrawableComponent<Sprite>
    {
        private int m_NumOfPermittedBulletsInScreen;
        protected Game m_Game;
        private List<Bullet> m_Bullets;
        private Sprite m_GunOwner;
        private string m_ShootCueName;
        private ISoundManager m_SoundManager;

        protected string ShootCueName
        {
            get { return m_ShootCueName; }
            set { m_ShootCueName = value; }
        }

        public Sprite GunOwner
        {
            get { return m_GunOwner; }
            set { m_GunOwner = value; }
        }

        public List<Bullet> Bullets
        {
            get { return m_Bullets; }
            set { m_Bullets = value; }
        }

        public int NumOfPermittedBulletsInScreen
        {
            get { return m_NumOfPermittedBulletsInScreen; }
            set { m_NumOfPermittedBulletsInScreen = value; }
        }

        public int BulletsCount
        {
            get { return m_Bullets.Count; }
        }

        public Gun(Game i_Game)
            : base(i_Game)
        {
            m_Game = i_Game;
            m_SoundManager = this.Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            m_Bullets = new List<Bullet>();
            m_NumOfPermittedBulletsInScreen = 0;
        }

        protected virtual bool CheckShootingConditions()
        {
            List<Bullet> bulletsToRemove = new List<Bullet>();

            foreach (Bullet bullet in m_Bullets)
            {
                if (IsBulletOutOfScreen(bullet))
                {
                    bulletsToRemove.Add(bullet);
                }
            }

            foreach(Bullet bullet in bulletsToRemove)
            {
                m_Bullets.Remove(bullet);
            }

            return m_Bullets.Count < m_NumOfPermittedBulletsInScreen && !this.GunOwner.IsDying;
        }

        protected abstract bool IsBulletOutOfScreen(Bullet i_Bullet);

        protected abstract void SetBulletProperties(Bullet i_Bullet);

        public void Shoot()
        {
            Bullet bullet;

            if (CheckShootingConditions())
            {
                bullet = new Bullet(m_Game, this.GunOwner);
                bullet.Initialize();
                SetBulletProperties(bullet);
                m_Bullets.Add(bullet);
                this.Add(bullet);
                m_SoundManager.PlayCue(this.ShootCueName);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
        }
    }
}
