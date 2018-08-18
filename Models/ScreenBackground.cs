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
    public class ScreenBackground : DrawableGameComponent
    {
        private string m_AssetName;
        protected Texture2D m_Texture;
        protected Color m_TintColor;
        protected SpriteBatch m_SpriteBatch;

        public string AssetName
        {
            get { return m_AssetName; }
            set { m_AssetName = value; }
        }

        protected SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
            set { m_SpriteBatch = value; }
        }

        protected Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public ScreenBackground(Game i_Game, Color i_TintColor, string i_AssetName)
            : base(i_Game)
        {
            this.AssetName = i_AssetName;
            this.TintColor = i_TintColor;
        }

        protected override void LoadContent()
        {
            this.Texture = Game.Content.Load<Texture2D>(this.AssetName);
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime i_GameTime)
        {
            Rectangle position;

            position = new Rectangle(0, 0, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.SpriteBatch.Begin();
            this.SpriteBatch.Draw(Texture, position, this.TintColor);
            this.SpriteBatch.End();
        }
    }
}
