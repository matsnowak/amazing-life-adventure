using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LifeFall.Logic.Blood
{
    public class GreenDiamond : CollectiblePoint
    {
        public GreenDiamond() : base()
        {
            this.CModel.textureEnabled = true;

            this.CModel.texture = Costam.greenDiamondTexture;
            BoundingSphere bs = (BoundingSphere)CModel.Model.Tag;
            bs.Radius = Costam.DiamondBoundingSphereRadius;
            CModel.Model.Tag = bs;
            

        }

        public override void OnCollision(Core.Player player)
        {
            Costam.collectEffect.Play();
            player.score += 3;

        }

        public override void OnCollision(Bullet bullet)
        {

        }
    }
}
