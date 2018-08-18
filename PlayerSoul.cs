using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex01_Yakir_201049475_Omer_300471430.Enums;

namespace C16_Ex01_Yakir_201049475_Omer_300471430
{
    public class PlayerSoul : Sprite
    {
        private const string k_AssetName = @"GameAssets\Ship01_32x32";
        private ePlayerType m_PlayerType;
        private int m_Index;

        public PlayerSoul(Game i_Game, Color i_TintColor,ePlayerType i_PlayerType, int i_Index)
            : base(i_Game, k_AssetName)
        {
            AssetName = k_AssetName;
            TintColor = i_TintColor;
            m_PlayerType = i_PlayerType;
            m_Index = i_Index;
            LayerDepth = 0.5f;
            Scales = new Vector2(0.5f);
            i_Game.Components.Add(this);         
        }


        protected override void InitPosition()
        {
            float x;
            float y;

            base.InitPosition();
            x = (float)GraphicsDevice.Viewport.Width - m_Index * this.Width * 1.5f - this.Width * 1.5f;
            y = (int)m_PlayerType * this.Height * 1.5f + this.Height / 2 ;
            
            this.Position = new Vector2(x, y);
        }
    }
}
