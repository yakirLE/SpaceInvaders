using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;
using C16_Ex03_Yakir_201049475_Omer_300471430.CustomEventArgs;
using C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.Models.Menu;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Managers
{
    public class MenuMananger : StackMananger<Menu>
    {
        public event EventHandler<EventArgs> Quit;

        private bool m_DidRemoveMainMenuQuitItem = false;

        public MenuMananger(Game i_Game)
            : base(i_Game)
        {
        }

        protected virtual void OnQuit()
        {
            if(Quit != null)
            {
                Quit.Invoke(this, EventArgs.Empty);
            }
        }

        public override void SetCurrentItem(Menu i_Item)
        {
            base.SetCurrentItem(i_Item);
            if(!m_DidRemoveMainMenuQuitItem)
            {
                this.ActiveItem.Remove(this.ActiveItem.Exit);
                m_DidRemoveMainMenuQuitItem = true;
            }
        }

        protected override void ActivateItem(Menu i_Menu)
        {
            i_Menu.Activate();
        }

        protected override void SetManager(Menu i_Menu)
        {
            i_Menu.MenuManager = this;
        }

        protected override void SubscribeToCloseEvent(Menu i_Menu)
        {
            i_Menu.Closed += menu_Closed;
        }

        private void menu_Closed(object sender, EventArgs e)
        {
            Item_Closed(sender, e);
        }

        protected override void UnsubscribeToCloseEvent(GameComponentEventArgs<Menu> e)
        {
            e.GameComponent.Closed -= menu_Closed;
        }

        protected override void SetPreviousItem(Menu i_Menu)
        {
            i_Menu.PreviousMenu = this.ActiveItem;
        }

        protected override void DeactivateCurrentItem()
        {
            this.ActiveItem.Deactivate();
        }

        protected override void ActivateCurrentItem()
        {
            this.ActiveItem.Activate();
        }
    }
}
