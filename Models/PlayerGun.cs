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
    public class PlayerGun : Gun
    {
        public PlayerGun(Game i_Game, Sprite i_GunOwner)
            : base(i_Game)
        {
            this.NumOfPermittedBulletsInScreen = 2;
            this.GunOwner = i_GunOwner;
        }

        protected override bool IsBulletOutOfScreen(Bullet i_Bullet)
        {
            return i_Bullet.Position.Y < -i_Bullet.Height;
        }

        protected override void SetBulletProperties(Bullet i_Bullet)
        {
            i_Bullet.Team = eTeam.Player;
            i_Bullet.TintColor = Color.DarkRed;
            i_Bullet.Position = new Vector2(this.GunOwner.Position.X + (this.GunOwner.Width / 2), this.GunOwner.Position.Y - i_Bullet.Height);
            this.ShootCueName = "SSGunShot";
        }
    }
}
