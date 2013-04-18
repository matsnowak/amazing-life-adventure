using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LifeFall.Debug
{
    public class HUDText : IHUDComponent
    {
        protected StringBuilder sb;
        SpriteFont font;

        int padding = 5; // Odleglosc tekstu od krawędzi

        Texture2D backgroundTexture;

        public bool DrawBackground = true;
        public bool AutoSize = true;

        Color backgroundColor = new Color(100, 100, 100, 100);
        public Color FontColor = Color.Black;


        public HUDText(Rectangle Position, SpriteFont font)
        {
            this.Position = Position;
            sb = new StringBuilder();
            this.font = font;

            backgroundTexture = new Texture2D(Costam.Game.GraphicsDevice, 1, 1);
            backgroundTexture.SetData(new[] { backgroundColor });
        }
        public Rectangle Position
        {
            get;
            set;
        }


        String textToDisplay;
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            textToDisplay = sb.ToString();
            if (DrawBackground)
            {
                if (AutoSize)
                {
                    Vector2 textDim = font.MeasureString(textToDisplay);

                    spriteBatch.Draw(backgroundTexture, new Rectangle(Position.X, Position.Y, (int) textDim.X + 2*padding, (int) textDim.Y + 2*padding), backgroundColor);
                }
                else
                {
                    spriteBatch.Draw(backgroundTexture, Position, backgroundColor);
                }
            }
            spriteBatch.DrawString(font, textToDisplay, new Vector2(Position.X + padding, Position.Y + padding), FontColor);
            sb.Clear();
            
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public void AddLine(String line)
        {
            sb.AppendLine(line);
        }
    }
}
