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
    public class SubMenuItem : MenuItem
    {
        protected MainMenu m_ContainedSubMenu;

        public MainMenu ContainedSubMenu
        {
            get { return m_ContainedSubMenu; }
        }

        public SubMenuItem(Game i_Game, string i_ItemName)
            : base(i_Game, i_ItemName)
        {
            m_ContainedSubMenu = new MainMenu();
        }
    }
}
