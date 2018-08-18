using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.Models.Menu
{
    public class MenuItem : CompositeDrawableComponent<DrawableGameComponent>
    {
        protected const float k_ScaleDownText = 3.5f;
        private string m_ItemFontName;
        private string m_ItemName;
        private bool m_HasFocus;
        private Color m_ActiveColor;
        private Color m_InactiveColor;
        private Text m_ItemNameText;
        private Menu m_MyMenu;

        public Menu MyMenu
        {
            get { return m_MyMenu; }
            set { m_MyMenu = value; }
        }

        public string ItemFontName
        {
            get { return m_ItemFontName; }
            set 
            {
                this.ItemNameText.FontName = value;
                m_ItemFontName = value; 
            }
        }

        public Text ItemNameText
        {
            get { return m_ItemNameText; }
            set { m_ItemNameText = value; }
        }

        public Color InactiveColor
        {
            get { return m_InactiveColor; }
            set { m_InactiveColor = value; }
        }

        public Color ActiveColor
        {
            get { return m_ActiveColor; }
            set { m_ActiveColor = value; }
        }

        protected string ItemName
        {
            get { return m_ItemName; }
            set { m_ItemName = value; }
        }

        public bool HasFocus
        {
            get { return m_HasFocus; }
            set { m_HasFocus = value; }
        }

        public MenuItem(Game i_Game, string i_ItemName)
            : base(i_Game)
        {
            m_ItemName = i_ItemName;
            this.HasFocus = false;
            m_ItemNameText = new Text(i_Game);
            m_ItemNameText.StringToPrint = m_ItemName;
            m_ItemNameText.Scales *= 1 / k_ScaleDownText;
            this.InactiveColor = m_ItemNameText.TintColor;
            this.ActiveColor = Color.LightGreen;
            this.ItemFontName = @"Fonts\Ravie_72";
            this.Add(m_ItemNameText);
        }

        protected int Mod(int a, int n)
        {
            return ((a % n) + n) % n;
        }

        public virtual void ActivateChosenItem()
        {
            MyMenu.ExitMenu();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.HasFocus)
            {
                m_ItemNameText.TintColor = this.ActiveColor;
            }
            else
            {
                m_ItemNameText.TintColor = this.InactiveColor;
            }

            base.Update(i_GameTime);
        }
    }
}
