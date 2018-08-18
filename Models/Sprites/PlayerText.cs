using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Enums;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class PlayerText : Text
    {
        private const float k_ConstantAdditiveDistance = 5;
        private ePlayerType m_PlayerType;

        public new Vector2 Position
        {
            get
            {
                return new Vector2(k_ConstantAdditiveDistance, (this.MeasureFont.Y * (float)m_PlayerType) + k_ConstantAdditiveDistance);
            }

            set { m_Position = value; }
        }

        public PlayerText(Game i_Game, ePlayerType i_PlayerType, Color i_TintColor)
            : base(i_Game, i_TintColor)
        {
            m_PlayerType = i_PlayerType;
        }

        public PlayerText(Game i_Game, ePlayerType i_PlayerType, string i_FontName, Color i_TintColor)
            : base(i_Game, i_FontName, i_TintColor)
        {
            m_PlayerType = i_PlayerType;
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Position = this.Position;
            base.Draw(i_GameTime);
        }
    }
}
