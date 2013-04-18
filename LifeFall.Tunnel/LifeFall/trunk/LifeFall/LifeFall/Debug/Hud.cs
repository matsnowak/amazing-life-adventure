using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace LifeFall.Debug
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Hud : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Dictionary<string, IHUDComponent> components;
        SpriteBatch spriteBatch;

        public Hud(Game game, SpriteBatch spriteBatch):
            base(game)
        {
            components = new Dictionary<string, IHUDComponent>();
            this.spriteBatch = spriteBatch;
        }

        public override void Initialize()
        {

           
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (IHUDComponent hc in components.Values)
            {
                hc.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            spriteBatch.Begin();

            foreach (IHUDComponent hc in components.Values)
            {
                hc.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
            //base.Draw(gameTime);
        }

        public void AddComponent(String name, IHUDComponent component)
        {
            components.Add(name, component);
        }

        public IHUDComponent GetComponent(String name)
        {
            return components[name];
        }
    }
}
