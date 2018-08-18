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
    public class GameOverSprite : Sprite
    {
        private const string k_AssetName = @"GameAssets\GameOverMessage";

        public GameOverSprite(Game i_Game)
            : base(i_Game, k_AssetName)
        {
            this.TintColor = Color.Red;
        }

        protected override void InitOrigins()
        {
            this.PositionOrigin = this.TextureCenter;
        }

        protected override void InitPosition()
        {
            int y;
            int x;
            Viewport viewport;
            
            base.InitPosition();
            viewport = this.Game.GraphicsDevice.Viewport;
            x = viewport.Width / 2;
            y = (viewport.Height / 2) - (viewport.Height / 4);
            this.Position = new Vector2(x, y);
        }
    }
}
