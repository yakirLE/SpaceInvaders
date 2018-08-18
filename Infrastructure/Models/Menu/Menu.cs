using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;
using C16_Ex03_Yakir_201049475_Omer_300471430.Managers;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.Models.Menu
{
    public class Menu : MenuItem
    {
        public event EventHandler<EventArgs> Closed;

        private MenuItem m_Back;
        private Menu m_PreviousMenu;
        private bool m_MenuHasFocus;
        private IInputManager m_InputManager;
        private IInputManager m_DummyInputManager;
        private int m_FocusedItemIndex;
        private IStackMananger<Menu> m_MenuManager;
        private ISoundManager m_SoundManager;

        public IStackMananger<Menu> MenuManager
        {
            get { return m_MenuManager; }
            set { m_MenuManager = value; }
        }

        public IInputManager InputManager
        {
            get { return this.MenuHasFocus ? m_InputManager : m_DummyInputManager; }
        }

        public bool MenuHasFocus
        {
            get { return m_MenuHasFocus; }
            set { m_MenuHasFocus = value; }
        }

        public Menu PreviousMenu
        {
            get { return m_PreviousMenu; }
            set { m_PreviousMenu = value; }
        }

        public MenuItem Exit
        {
            get { return m_Back; }
            set { m_Back = value; }
        }

        public Menu(Game i_Game, string i_ItemName)
            : base(i_Game, i_ItemName)
        {
            m_SoundManager = this.Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            Deactivate();
            this.MyMenu = null;
            m_DummyInputManager = new DummyInputManager();
            initMenuLabel(i_ItemName);
            m_Back = InitItem("Back");
            this.Add(m_Back);
        }

        protected MenuItem InitItem(string i_Name)
        {
            MenuItem menuItem;

            menuItem = new MenuItem(this.Game, i_Name);
            menuItem.DrawOrder = int.MaxValue;
            menuItem.MyMenu = this;

            return menuItem;
        }

        private void modifyBackItem()
        {
            this.Exit.DrawOrder = int.MaxValue;
            this.Exit.MyMenu = this;
        }

        private void modifyBackItem(string i_Name)
        {
            this.Exit.ItemNameText.StringToPrint = i_Name;
            modifyBackItem();
        }

        private void initMenuLabel(string i_Name)
        {
            this.ItemNameText.FontName = this.ItemFontName;
            this.ItemNameText.DrawOrder = int.MinValue;
            this.ItemNameText.StringToPrint = i_Name;
        }

        public void Show()
        {
            int itemIndex;
            float y;
            Viewport viewport;

            viewport = this.Game.GraphicsDevice.Viewport;
            itemIndex = 0;
            this.ItemNameText.Position = new Vector2(viewport.Width / 22, viewport.Height / 20);
            foreach (MenuItem item in this.m_DrawableComponents.OfType<MenuItem>())
            {
                y = item.ItemNameText.Height * itemIndex++;
                item.ItemNameText.Position = new Vector2(viewport.Width / 7, (viewport.Height / 6) + y);
            }
        }

        public void AddItem(MenuItem i_ItemToAdd)
        {
            Menu asMainMenu;

            asMainMenu = i_ItemToAdd as Menu;
            if (asMainMenu != null)
            {
                asMainMenu.initMenuLabel(asMainMenu.ItemName);
                asMainMenu.modifyBackItem("Done");
                asMainMenu.MenuManager = this.MenuManager;
            }

            i_ItemToAdd.MyMenu = this;
            i_ItemToAdd.DrawOrder = this.Count;
            this.Add(i_ItemToAdd);
        }

        public override void ActivateChosenItem()
        {
            if (this.HasFocus)
            {
                this.MenuManager.SetCurrentItem(this);
            }
        }

        public override void Initialize()
        {
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            if (m_InputManager == null)
            {
                m_InputManager = m_DummyInputManager;
            }

            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if(this.MenuHasFocus)
            {
                handleMenuInputs();
            }
        }

        private void handleMenuInputs()
        {
            bool didMoveInMenu;
            ToggleMenuItem toggleItem;

            didMoveInMenu = false;
            Show();
            if (this.InputManager.IsKeyPressed(Keys.Down))
            {
                (this.m_DrawableComponents[m_FocusedItemIndex] as MenuItem).HasFocus = false;
                m_FocusedItemIndex++;
                m_FocusedItemIndex = Mod(m_FocusedItemIndex, this.m_DrawableComponents.Count);
                (this.m_DrawableComponents[m_FocusedItemIndex] as MenuItem).HasFocus = true;
                didMoveInMenu = true;
            }
            else if (this.InputManager.IsKeyPressed(Keys.Up))
            {
                (this.m_DrawableComponents[m_FocusedItemIndex] as MenuItem).HasFocus = false;
                m_FocusedItemIndex--;
                m_FocusedItemIndex = Mod(m_FocusedItemIndex, this.m_DrawableComponents.Count);
                (this.m_DrawableComponents[m_FocusedItemIndex] as MenuItem).HasFocus = true;
                didMoveInMenu = true;
            }
            else if (this.InputManager.IsKeyPressed(Keys.Enter))
            {
                MenuItem menuItem;

                menuItem = this.m_DrawableComponents[m_FocusedItemIndex] as MenuItem;
                if (!(menuItem is ToggleMenuItem))
                {
                    menuItem.ActivateChosenItem();
                    didMoveInMenu = true;
                }
            }
            else if (this.InputManager.IsKeyPressed(Keys.PageDown))
            {
                toggleItem = this.m_DrawableComponents[m_FocusedItemIndex] as ToggleMenuItem;
                if (toggleItem != null)
                {
                    toggleItem.SingleMoveDirectionInMenu++;
                    toggleItem.ActivateChosenItem();
                    didMoveInMenu = true;
                }
            }
            else if (this.InputManager.IsKeyPressed(Keys.PageUp))
            {
                toggleItem = this.m_DrawableComponents[m_FocusedItemIndex] as ToggleMenuItem;
                if (toggleItem != null)
                {
                    toggleItem.SingleMoveDirectionInMenu--;
                    toggleItem.ActivateChosenItem();
                    didMoveInMenu = true;
                }
            }

            if (didMoveInMenu)
            {
                m_SoundManager.PlayCue("MenuMove");
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (this.MenuHasFocus)
            {
                base.Draw(i_GameTime);
                foreach (Menu menu in this.OfType<Menu>())
                {
                    menu.ItemNameText.SpriteBatch.Begin();
                    menu.ItemNameText.Draw(i_GameTime);
                    menu.ItemNameText.SpriteBatch.End();
                }
            }
        }

        public void Deactivate()
        {
            this.Enabled = this.Visible = this.MenuHasFocus = false;
            foreach (MenuItem item in this.OfType<MenuItem>())
            {
                item.Enabled = item.Visible = item.HasFocus = false;
            }
        }

        public void Activate()
        {
            bool firstItemGotFocus;

            firstItemGotFocus = false;
            this.Enabled = this.Visible = this.MenuHasFocus = true;
            foreach (MenuItem item in this.m_DrawableComponents.OfType<MenuItem>())
            {
                if(!firstItemGotFocus)
                {
                    m_FocusedItemIndex = 0;
                    item.HasFocus = true;
                    firstItemGotFocus = true;
                }

                item.Enabled = item.Visible = true;
            }

            OnActivated();
        }

        protected virtual void OnActivated()
        {
            if (this.PreviousMenu != null && this.MenuHasFocus)
            {
                this.PreviousMenu.MenuHasFocus = false;
            }
        }

        public void ExitMenu()
        {
            Deactivate();
            OnClosed();
        }

        protected virtual void OnClosed()
        {
            if (Closed != null)
            {
                Closed.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
