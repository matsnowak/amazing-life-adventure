using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LifeFall.Logic.Blood
{
    public class CollectiblePoint : MovableObject
    {
        public CollectiblePoint()
            : base(Costam.DiamondModel)
        {
            this.CModel.texture = Costam.BloodCellTexture;
            BoundingSphere bs = (BoundingSphere)CModel.Model.Tag;
            bs.Radius = Costam.DiamondBoundingSphereRadius;
        }

        public override void OnCollision(Core.Player player)
        {
           
        }
        public override void OnCollision(Bullet bullet)
        {
            
        }
    }
}
