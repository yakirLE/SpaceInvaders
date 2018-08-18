using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.CustomEventArgs;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.Models.Menu
{
    public class ToggleMenuItem : ActivatableMenuItem
    {
        public new event EventHandler<MenuToggleItemEventArgs> ItemChosen;

        private List<object> m_ToggleItems;
        private int m_CurrentToggleItemIndex;
        private int m_SingleMoveDirectionInMenu;
        private MenuToggleItemEventArgs m_MenuToggleItemEventArgs;

        public int SingleMoveDirectionInMenu
        {
            get { return m_SingleMoveDirectionInMenu; }
            set { m_SingleMoveDirectionInMenu = value; }
        }

        public ToggleMenuItem(Game i_Game, string i_ItemName, List<object> i_ToggleItems)
            : base(i_Game, i_ItemName)
        {
            m_MenuToggleItemEventArgs = new MenuToggleItemEventArgs();
            m_CurrentToggleItemIndex = 0;
            m_ToggleItems = new List<object>();
            m_ToggleItems = i_ToggleItems;
            this.ItemNameText.StringToPrint = string.Format("{0}:   {1}", this.ItemName, m_ToggleItems[m_CurrentToggleItemIndex].ToString());
        }

        public override void ActivateChosenItem()
        {
            m_CurrentToggleItemIndex = Mod(this.SingleMoveDirectionInMenu, m_ToggleItems.Count);
            this.ItemNameText.StringToPrint = string.Format("{0}:   {1}", this.ItemName, m_ToggleItems[m_CurrentToggleItemIndex].ToString());
            m_MenuToggleItemEventArgs.CurrentToggleItem = m_ToggleItems[m_CurrentToggleItemIndex];
            OnItemChosen();
        }

        protected override void OnItemChosen()
        {
            if (ItemChosen != null && this.HasFocus)
            {
                ItemChosen.Invoke(this, m_MenuToggleItemEventArgs);
            }
        }
    }
}
