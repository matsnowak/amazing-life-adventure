using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Debug
{
    public class DebugConsole: HUDText
    {
        bool showConsole = true;


        public DebugConsole(Rectangle Position, SpriteFont font): base(Position, font)
        {
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.H))
            {
                showConsole = !showConsole;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (showConsole)
            {
                base.Draw(spriteBatch, gameTime);
            }
        }
    }
}
