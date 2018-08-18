using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Interfaces
{
    public interface ICollidable
    {
        event EventHandler<EventArgs> PositionChanged;

        event EventHandler<EventArgs> SizeChanged;

        event EventHandler<EventArgs> VisibleChanged;

        event EventHandler<EventArgs> Disposed;

        bool Visible { get; }

        bool CheckCollision(ICollidable i_Source);

        void Collided(ICollidable i_Collidable);
    }
}
