using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.Models.Menu;
using C16_Ex03_Yakir_201049475_Omer_300471430.Managers;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;
using C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.CustomEventArgs;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Screens
{
    public class MenuScreen : GameScreen
    {
        private GraphicsDeviceManager m_Graphics;
        private Menu m_MainMenu;
        private IGameManager m_GameManager;
        private ISoundManager m_SoundManager;

        public GraphicsDeviceManager Graphics
        {
            get { return m_Graphics; }
            set { m_Graphics = value; }
        }

        public MenuScreen(Game i_Game)
            : base(i_Game)
        {
            List<object> volumeHops, offOnList;
            ScreenBackground background = new ScreenBackground(i_Game, Color.DarkCyan, @"GameAssets\BG_Space01_1024x768");
            MenuMananger menuManager;
            Menu soundMenu, screenMenu;
            ActivatableMenuItem play, quit;
            ToggleMenuItem players;
            ToggleMenuItem backgroundMusicVolume, soundEffectsVolume, toggleSound;
            ToggleMenuItem mouseVisability, allowWindowResizing, fullScreenMode;

            m_SoundManager = this.Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            volumeHops = new List<object> { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 0 };
            offOnList = new List<object> { "Off", "On" };
            m_GameManager = i_Game.Services.GetService(typeof(IGameManager)) as IGameManager;
            menuManager = new MenuMananger(i_Game);
            m_MainMenu = new Menu(i_Game, "Main Menu");
            soundMenu = new Menu(i_Game, "Sound Options");
            backgroundMusicVolume = new ToggleMenuItem(i_Game, "Background Music Volume", volumeHops);
            soundEffectsVolume = new ToggleMenuItem(i_Game, "Sound Effects Volume", volumeHops);
            toggleSound = new ToggleMenuItem(i_Game, "Toggle Sound", new List<object> { "On", "Off" });
            screenMenu = new Menu(i_Game, "Screen Options");
            mouseVisability = new ToggleMenuItem(i_Game, "Mouse Visability", new List<object> { "Invisible", "Visible" });
            allowWindowResizing = new ToggleMenuItem(i_Game, "Allow Window Resizing", offOnList);
            fullScreenMode = new ToggleMenuItem(i_Game, "Full Screen Mode", offOnList);
            players = new ToggleMenuItem(i_Game, "Players", new List<object> { "One", "Two" });
            play = new ActivatableMenuItem(i_Game, "Play");
            quit = new ActivatableMenuItem(i_Game, "Quit");
            m_MainMenu.MenuManager = menuManager;
            soundMenu.AddItem(backgroundMusicVolume);
            soundMenu.AddItem(soundEffectsVolume);
            soundMenu.AddItem(toggleSound);
            screenMenu.AddItem(mouseVisability);
            screenMenu.AddItem(allowWindowResizing);
            screenMenu.AddItem(fullScreenMode);
            m_MainMenu.AddItem(soundMenu);
            m_MainMenu.AddItem(screenMenu);
            m_MainMenu.AddItem(players);
            m_MainMenu.AddItem(play);
            m_MainMenu.AddItem(quit);
            backgroundMusicVolume.ItemChosen += backgroundMusicVolumeToggleItem_ItemChosen;
            soundEffectsVolume.ItemChosen += soundEffectsVolumeToggleItem_ItemChosen;
            toggleSound.ItemChosen += toggleSoundToggleItem_ItemChosen;
            mouseVisability.ItemChosen += mouseVisabilityToggleItem_ItemChosen;
            allowWindowResizing.ItemChosen += allowWindowResizingToggleItem_ItemChosen;
            fullScreenMode.ItemChosen += fullScreenModeToggleItem_ItemChosen;
            players.ItemChosen += playersToggleItem_ItemChosen;
            play.ItemChosen += playItem_ItemChosen;
            quit.ItemChosen += quitItem_ItemChosen;
            menuManager.SetCurrentItem(m_MainMenu);
            this.Add(background);
            this.Add(menuManager);
        }

        private void quitItem_ItemChosen(object sender, EventArgs e)
        {
            this.Game.Exit();
        }

        private void playItem_ItemChosen(object sender, EventArgs e)
        {
            ExitScreen();
            m_GameManager.WasGameOverInitiated = false;
            this.ScreensManager.Push(new PlayScreen(this.Game));
            this.ScreensManager.SetCurrentItem(new LevelTransitionScreen(this.Game));
        }

        private void fullScreenModeToggleItem_ItemChosen(object sender, MenuToggleItemEventArgs e)
        {
            if (this.Graphics != null)
            {
                this.Graphics.ToggleFullScreen();
            }
        }

        private void allowWindowResizingToggleItem_ItemChosen(object sender, MenuToggleItemEventArgs e)
        {
            this.Game.Window.AllowUserResizing = e.CurrentToggleItem.ToString() == "On";
        }

        private void mouseVisabilityToggleItem_ItemChosen(object sender, MenuToggleItemEventArgs e)
        {
            this.Game.IsMouseVisible = e.CurrentToggleItem.ToString() == "Visible";
        }

        private void toggleSoundToggleItem_ItemChosen(object sender, MenuToggleItemEventArgs e)
        {
            m_SoundManager.ToggleMute();
        }

        private float convertVolumeFromDecimalToPercentage(int i_CurrentToggleItem)
        {
            float volumeToSet;

            volumeToSet = i_CurrentToggleItem == 0 ? 0 : (float)i_CurrentToggleItem / 100;

            return volumeToSet;
        }

        private void backgroundMusicVolumeToggleItem_ItemChosen(object sender, MenuToggleItemEventArgs e)
        {
            m_SoundManager.SetBackgroundMusicVolume(convertVolumeFromDecimalToPercentage((int)e.CurrentToggleItem));
        }
            
        private void soundEffectsVolumeToggleItem_ItemChosen(object sender, MenuToggleItemEventArgs e)
        {
            m_SoundManager.SetSoundEffectsVolume(convertVolumeFromDecimalToPercentage((int)e.CurrentToggleItem));
        }
        
        private void playersToggleItem_ItemChosen(object sender, MenuToggleItemEventArgs e)
        {
            if(e.CurrentToggleItem.ToString() == "One")
            {
                m_GameManager.PlayersAmount = 1;
            }
            else
            {
                m_GameManager.PlayersAmount = 2;
            }
        }
    }
}
