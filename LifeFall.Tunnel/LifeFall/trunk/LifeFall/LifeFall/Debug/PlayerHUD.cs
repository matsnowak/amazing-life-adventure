using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeFall.Core;

namespace LifeFall.Debug
{
    public class PlayerHUD : HUDText
    {
        bool ShowFPS = true;

        Player player;

        public PlayerHUD(Rectangle position, SpriteFont font, Player player)
            : base(position, font)
        {
            DrawBackground = true;
            FontColor = Color.Red;
            this.player = player;
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

            this.AddLine(player.nickName);
            this.AddLine("Health: " + player.health.ToString());
            this.AddLine("Score: " + player.score.ToString());

            if (ShowFPS)
            {
                base.Draw(spriteBatch, gameTime);
            }
        }
    }
}
