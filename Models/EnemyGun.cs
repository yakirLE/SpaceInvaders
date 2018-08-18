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
    public class EnemyGun : Gun
    {
        private static Random m_Random;
        private int m_RandomShootTime;
        private double m_TimeSinceLastShoot;
        private bool m_IsTimestampSaved;
        private GameTime m_GameTime;

        public GameTime GameTime
        {
            set { m_GameTime = value; }
        }   

        public EnemyGun(Game i_Game, Sprite i_GunOwner)
            : base(i_Game)
        {
            IGameManager gameManager;

            gameManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;
            m_Random = new Random();
            m_RandomShootTime = 0;
            m_IsTimestampSaved = false;
            m_TimeSinceLastShoot = 0;
            this.NumOfPermittedBulletsInScreen = gameManager.RemainderPlusLevelCycle + 1;
            this.GunOwner = i_GunOwner;
        }

        private bool checkIfTimeToShoot()
        {
            bool timeToShoot = false;

            if (!m_IsTimestampSaved)
            {
                m_RandomShootTime = m_Random.Next(1, 30);
                m_TimeSinceLastShoot = m_GameTime.TotalGameTime.TotalSeconds;
                m_IsTimestampSaved = true;
            }

            if (m_GameTime.TotalGameTime.TotalSeconds - m_TimeSinceLastShoot >= (double)m_RandomShootTime)
            {
                timeToShoot = true;
                m_IsTimestampSaved = false;
            }

            return timeToShoot;
        }

        protected override bool CheckShootingConditions()
        {
            bool areNoBulletsInScreen;

            areNoBulletsInScreen = base.CheckShootingConditions();

            return areNoBulletsInScreen && checkIfTimeToShoot() && (this.GunOwner as EnemySpaceShip).Visible;
        }

        protected override bool IsBulletOutOfScreen(Bullet i_Bullet)
        {
            return i_Bullet.Position.Y >= m_Game.GraphicsDevice.Viewport.Height;
        }

        protected override void SetBulletProperties(Bullet i_Bullet)
        {
            i_Bullet.Team = eTeam.Enemy;
            i_Bullet.TintColor = Color.Blue;
            i_Bullet.Direction = new Vector2(0, 1);
            i_Bullet.Velocity = i_Bullet.Speed * i_Bullet.Direction;
            i_Bullet.Position = new Vector2(this.GunOwner.Position.X + (this.GunOwner.Width / 2), this.GunOwner.Position.Y + i_Bullet.Height);
            this.ShootCueName = "EnemyGunShot";
        }
    }
}
