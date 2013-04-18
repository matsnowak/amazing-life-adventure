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


namespace LifeFall.Core.Trigger_System
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TriggerManager : Microsoft.Xna.Framework.GameComponent
    {

        List<Trigger> triggers = new List<Trigger>();

        public TriggerManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (Trigger T in triggers)
            {
                if (T.Enabled)
                    T.CheckTrigger();
            }

            base.Update(gameTime);
        }

        public void AddTrigger(Trigger trigger)
        {
            triggers.Add(trigger);
        }

        public void RemoveTrigger(Trigger trigger)
        {
            triggers.Remove(trigger);
        }

        
    }
}
