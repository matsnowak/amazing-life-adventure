using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Debug
{
    public class FPSCounter: HUDText
    {
        bool ShowFPS = true;
        int counter = 0;
        double timeCounter = 0;
        int lastSecondCounter = 0;

        public FPSCounter(Rectangle position, SpriteFont font): base(position, font)
        {
            DrawBackground = false;
            FontColor = Color.White;
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.F))
            {
                ShowFPS = !ShowFPS;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= 1.0f)
            {
                timeCounter -= 1;
                lastSecondCounter = counter;
                counter = 0;

            }
            sb.Append("fps: " + lastSecondCounter.ToString());
            ++counter;

            if (ShowFPS)
            {
                base.Draw(spriteBatch, gameTime);
            }
        }
    }
}
