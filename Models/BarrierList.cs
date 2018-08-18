using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
    public class BarrierList : CompositeDrawableComponent<Sprite>
    {
        private const int k_AmountOfBarriers = 4;
        private List<Barrier> m_BarrierList;
        private Color m_TintColor;
        private bool m_DidInitPosition;

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public BarrierList(Game i_Game, Color i_TintColor)
            : base(i_Game)
        {
            m_TintColor = i_TintColor;
            m_BarrierList = new List<Barrier>(k_AmountOfBarriers);
            m_DidInitPosition = false;
        }

        private void initPosition()
        {
            int idx = 0;
            float x;
            float y;
            float spaceShipHeight;

            spaceShipHeight = PlayerSpaceShip.m_PlayerSpaceShipHeight;
            fillList();
            foreach(Barrier barrier in this)
            {
                x = (Game.GraphicsDevice.Viewport.Width / 2) - ((k_AmountOfBarriers - 0.5f) * barrier.Width) + (idx++ * barrier.Width * 2);
                y = Game.GraphicsDevice.Viewport.Height - (spaceShipHeight + (barrier.Height * 2));
                barrier.Position = barrier.InitialPosition = new Vector2(x, y);
            }
        }

        private void fillList()
        {
            for (int i = 0; i < k_AmountOfBarriers; i++)
            {
                this.Add(new Barrier(Game, this.TintColor));
            }
        }

        public override void Initialize()
        {            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(!m_DidInitPosition)
            {
                initPosition();
                m_DidInitPosition = true;
            }
        }
    }
}
