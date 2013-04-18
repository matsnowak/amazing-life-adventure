using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic
{
    public class Behaviour
    {
        public delegate void execute(GameTime gameTime);
        public execute Execute { get; set; }

        public MovableObject Owner;

        public Behaviour()
        {
            
        }

        public virtual void Init(MovableObject owner)
        {
            this.Owner = owner;
            //Owner.AutoRotate = false;
           
        }

        public virtual Behaviour GetCopy()
        {
            return new Behaviour();
        }
    }
}
