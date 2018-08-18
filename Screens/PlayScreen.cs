using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Screens
{
    public class PlayScreen : GameScreen
    {
        private PauseScreen m_PauseScreen;
        private EnemyMatrix m_EnemyMatrix;
        private BarrierList m_BarrierList;

        public PlayScreen(Game i_Game)
            : base(i_Game)
        {
            GameManager gameManager;
            ScreenBackground background = new ScreenBackground(i_Game, Color.DarkCyan, @"GameAssets\BG_Space01_1024x768");
            MotherShip motherShip = new MotherShip(i_Game, Color.Red);

            gameManager = this.Game.Services.GetService(typeof(IGameManager)) as GameManager;
            gameManager.LevelPassed += gameManager_LevelPassed;
            gameManager.GameOver += gameManager_GameOver;
            m_EnemyMatrix = new EnemyMatrix(i_Game);
            m_BarrierList = new BarrierList(i_Game, Color.LightGreen);
            m_PauseScreen = new PauseScreen(i_Game);
            this.Add(background);
            this.Add(m_EnemyMatrix);
            createPlayers(gameManager);
            this.Add(motherShip);
            this.Add(m_BarrierList);
        }

        private void createPlayers(IGameManager i_GameManager)
        {
            IGameManager gameManager;
            Player player1;
            Player player2;

            gameManager = this.Game.Services.GetService(typeof(IGameManager)) as IGameManager;
            player1 = new Player(this.Game, Color.LightBlue, Enums.ePlayerType.First);
            this.Add(player1);
            if (gameManager.PlayersAmount == 2)
            {
                player2 = new Player(this.Game, Color.LightGreen, Enums.ePlayerType.Second);
                this.Add(player2);
            }
        }

        private void recreateEnemyMatrix()
        {
            removeEnemysBullets();
            this.Remove(m_EnemyMatrix);
            m_EnemyMatrix = new EnemyMatrix(this.Game);
            this.Add(m_EnemyMatrix);
        }

        private void recreateBarrierList()
        {
            foreach(Barrier barrier in m_BarrierList)
            {
                barrier.Dispose();
            }

            this.Remove(m_BarrierList);
            m_BarrierList = new BarrierList(this.Game, Color.LightGreen);
            this.Add(m_BarrierList);
        }

        private void removePlayersBullets()
        {
            foreach (Player player in this.OfType<Player>())
            {
                for (int i = 0; i < player.PlayerSpaceShip.PlayerGun.Bullets.Count; i++)
                {
                    player.PlayerSpaceShip.PlayerGun.Bullets[i].Dispose();
                }
            }
        }

        private void gameManager_LevelPassed(object sender, EventArgs e)
        {
            removePlayersBullets();
            recreateEnemyMatrix();
            recreateBarrierList();
            if (!(this.ScreensManager.ActiveItem is LevelTransitionScreen))
            {
                this.ScreensManager.SetCurrentItem(new LevelTransitionScreen(this.Game));
            }
        }

        private void removeEnemysBullets()
        {
            List<Bullet> bulletsToRemove;

            bulletsToRemove = new List<Bullet>();
            foreach (EnemySpaceShip enemy in m_EnemyMatrix.OfType<EnemySpaceShip>())
            {
                for (int i = 0; i < enemy.EnemyGun.Bullets.Count; i++)
                {
                    enemy.EnemyGun.Bullets[i].Dispose();
                }
            }
        }

        private void gameManager_GameOver(object sender, EventArgs e)
        {
            removeAllPlayers();
            removeEnemysBullets();
            this.Dispose();
            ExitScreen();
        }

        private void removeAllPlayers()
        {
            List<Player> playersToRemove;

            playersToRemove = new List<Player>();
            foreach (Player player in this.OfType<Player>())
            {
                playersToRemove.Add(player);
            }

            foreach (Player player in playersToRemove)
            {
                Remove(player);
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (this.InputManager.IsKeyPressed(Keys.P))
            {
                this.ScreensManager.SetCurrentItem(m_PauseScreen);
            }
        }
    }
}
