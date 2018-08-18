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

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class Bullet : Sprite, ICollidable2D, IUpdateGameManager
    {
        private const string k_AssetName = @"GameAssets\Bullet";
        private const float k_Speed = 110f;
        private Random m_Random = new Random();
        private Sprite m_BulletOwner;

        public Sprite BulletOwner
        {
            get { return m_BulletOwner; }
            set { m_BulletOwner = value; }
        }

        public Bullet(Game i_Game, Sprite i_BulletOwner)
            : base(i_Game, k_AssetName)
        {
            this.AssetName = k_AssetName;
            this.Direction = new Vector2(0, -1);
            Velocity = k_Speed * this.Direction;
            m_BulletOwner = i_BulletOwner;
            NotifyGameManagerAboutMe();
        }

        public float Speed
        {
            get { return k_Speed; }
        }

        protected override void Dispose(bool i_Disposing)
        {
            PlayerSpaceShip playerSpaceShip = m_BulletOwner as PlayerSpaceShip;
            EnemySpaceShip enemySpaceShip = m_BulletOwner as EnemySpaceShip;

            base.Dispose(i_Disposing);
            this.Visible = false;
            if (playerSpaceShip != null)
            {
                playerSpaceShip.RemoveBulletFromCollection(this);
            }
            else if (enemySpaceShip != null)
            {
                enemySpaceShip.RemoveBulletFromCollection(this);
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            bool shouldDie = true;
            Sprite asSprite = i_Collidable as Sprite;
            Bullet asBullet = i_Collidable as Bullet;
            MotherShip asMotherShip;
            EnemySpaceShip asEnemy;
            PlayerSpaceShip asPlayerSpaceShip;
            ScoreChangedEventArgs scoreChangedEventArgs;

            if(Team != asSprite.Team)
            {
                if(asBullet == null)
                {
                    Game.Components.Remove(this);
                    if (asSprite.Team == eTeam.Enemy)
                    {
                        this.Dispose();
                        asSprite.Dispose();
                    }
                }
                else if (m_BulletOwner is EnemySpaceShip && m_Random.Next(0, 2) == 0)
                {
                    shouldDie = false;
                }

                asEnemy = i_Collidable as EnemySpaceShip;
                asMotherShip = i_Collidable as MotherShip;
                asPlayerSpaceShip = this.BulletOwner as PlayerSpaceShip;
                if (asPlayerSpaceShip != null && (asEnemy != null || asMotherShip != null))
                {
                    scoreChangedEventArgs = new ScoreChangedEventArgs();
                    if (asEnemy != null)
                    {
                        scoreChangedEventArgs.EnemyPointsWorth = asEnemy.EnemyPointsWorth;
                    }
                    else
                    {
                        scoreChangedEventArgs.EnemyPointsWorth = asMotherShip.EnemyPointsWorth;
                    }

                    scoreChangedEventArgs.PlayerType = asPlayerSpaceShip.PlayerType;
                    scoreChangedEventArgs.DidHitEnemy = true;
                    OnScoreChanged(scoreChangedEventArgs);
                }

                if(shouldDie)
                {
                    base.Collided(i_Collidable);
                    OnRemoveMeAsNotifier();
                }
            }
        }
    }
}