using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.Models.Menu;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.CustomEventArgs
{
    public class GameComponentEventArgs<ComponentType> : EventArgs
        where ComponentType : IGameComponent
    {
        private ComponentType m_Component;

        public ComponentType GameComponent
        {
            get { return m_Component; }
        }

        public GameComponentEventArgs(ComponentType i_GameComponent)
        {
            m_Component = i_GameComponent;
        }
    }
}
