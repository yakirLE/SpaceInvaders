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
    public class Text : Sprite
    {
        protected const string k_FontName = @"Fonts\Ravie_72";
        private string m_FontName;
        protected SpriteFont m_Font;
        protected string m_StringToPrint;
        protected Vector2 m_FontCenter;

        protected Vector2 MeasureFont
        { 
            get
            {
                return m_Font.MeasureString(m_StringToPrint);
            }
        }

        public Vector2 FontCenter
        {
            get
            {
                return new Vector2(this.Width / 2, this.Height / 2);
            }
        }

        protected new float WidthBeforeScale
        {
            get { return this.MeasureFont.X; }
            set { m_WidthBeforeScale = value; }
        }

        protected new float HeightBeforeScale
        {
            get { return this.MeasureFont.Y; }
            set { m_HeightBeforeScale = value; }
        }

        public new float Height
        {
            get { return this.HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
        }

        public new float Width
        {
            get { return this.WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public string FontName
        {
            get { return m_FontName; }
            set { m_FontName = value; }
        }

        public string StringToPrint
        {
            get { return m_StringToPrint; }
            set { m_StringToPrint = value; }
        }

        public SpriteFont Font
        {
            get { return m_Font; }
            set { m_Font = value; }
        }

        public Text(Game i_Game)
            : this(i_Game, Color.LightBlue)
        {   
        }

        public Text(Game i_Game, Color i_TintColor)
            : base(i_Game, k_FontName)
        {
            this.TintColor = i_TintColor;
            m_StringToPrint = string.Empty;
            setFontNameIfNotSetAlready();
        }

        public Text(Game i_Game, string i_FontName, Color i_TintColor)
            : this(i_Game, i_TintColor)
        {
            this.FontName = i_FontName;
        }

        public Text(Game i_Game, string i_FontName)
            : this(i_Game)
        {
            this.FontName = i_FontName;
        }

        protected override void LoadObject()
        {
            this.Font = Game.Content.Load<SpriteFont>(this.FontName);
        }

        protected override void InitPosition()
        {
        }

        private void setFontNameIfNotSetAlready()
        {
            if(this.FontName == null)
            {
                this.FontName = k_FontName;
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (this.Visible)
            {
                if (!this.UseSharedBatch)
                {
                    this.SpriteBatch.Begin();
                }
                
                this.SpriteBatch.DrawString(
                    this.Font,
                    this.m_StringToPrint,
                    this.PositionForDraw,
                    this.TintColor,
                    this.Rotation,
                    this.RotationOrigin,
                    this.Scales,
                    this.SpriteEffects,
                    this.LayerDepth);
                if (!UseSharedBatch)
                {
                    this.SpriteBatch.End();
                }
            }
        }
    }
}
