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
    public interface ICollidable2D : ICollidable
    {
        Color[] TexturePixels { get; set; }

        List<Point> CollidedPixels { get; set; }

        Rectangle Boundry { get; }

        Vector2 Velocity { get; }
    }
}
