using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.Models.Menu
{
    public class ActivatableMenuItem : MenuItem
    {
        public event EventHandler<EventArgs> ItemChosen;

        public ActivatableMenuItem(Game i_Game, string i_ItemName)
            : base(i_Game, i_ItemName)
        {
        }

        public override void ActivateChosenItem()
        {
            OnItemChosen();
        }

        protected virtual void OnItemChosen()
        {
            if (ItemChosen != null && this.HasFocus)
            {
                ItemChosen.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
