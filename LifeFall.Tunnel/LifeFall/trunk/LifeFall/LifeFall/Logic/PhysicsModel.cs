using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jitter.Dynamics;
using Jitter.Collision.Shapes;
using Jitter.Collision;
using Jitter.LinearMath;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LifeFall.Logic
{
    public class PhysicsModel
    {
        Model model;

        public RigidBody RigidBody;
        Shape Shape;
       


        public PhysicsModel(Model model)
        {
            BoundingSphere b = (BoundingSphere)model.Tag;
            float SphereRadius = 20f; 
            if (b != null)
            {
                SphereRadius = b.Radius;
            }
            Shape = new SphereShape(SphereRadius);
            
            RigidBody = new RigidBody(Shape);
            this.model = model;

        }
    }
}
