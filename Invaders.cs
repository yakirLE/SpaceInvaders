using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Managers;
using C16_Ex03_Yakir_201049475_Omer_300471430.Screens;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    // $G$ SFN-007 (-5) Mute with 'M' button and sound-switch are not synchronized.

    // $G$ SFN-006 (-7) Enemies and spaceship die for no reason.


    public class Invaders : Game
    {
        private GraphicsDeviceManager m_Graphics;

        public Invaders()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            InputManager inputManager = new InputManager(this);
            ScreensMananger screensManager = new ScreensMananger(this);
            MenuMananger menuManager = new MenuMananger(this);
            SoundManager soundManager = new SoundManager(this);
            CollisionsManager collisionsManager = new CollisionsManager(this);
            GameManager gameManager = new GameManager(this);
            MenuScreen menuScreen = new MenuScreen(this);
            GameOverScreen gameOverScreen;
            WelcomeScreen welcomeScreen;

            menuScreen.Graphics = m_Graphics;
            gameOverScreen = new GameOverScreen(this);
            welcomeScreen = new WelcomeScreen(this);
            gameOverScreen.MenuScreen = welcomeScreen.MenuScreen = menuScreen;
            screensManager.Push(gameOverScreen);
            screensManager.SetCurrentItem(welcomeScreen);
        }

        protected override void Initialize()
        {       
            base.Initialize();
            this.Window.Title = "Invaders";
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime i_GameTime)
        {   
            base.Update(i_GameTime);
        }

        protected override void Draw(GameTime i_GameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(i_GameTime);
        }
    }
}
