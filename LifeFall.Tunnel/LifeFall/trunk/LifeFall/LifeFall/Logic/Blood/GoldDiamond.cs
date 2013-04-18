using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LifeFall.Logic.Blood
{
    public class GoldDiamond : CollectiblePoint
    {
        public GoldDiamond() : base()
        {
            this.CModel.textureEnabled = true;

            this.CModel.texture = Costam.goldDiamondTexture;
            BoundingSphere bs = (BoundingSphere)CModel.Model.Tag;
            bs.Radius = Costam.DiamondBoundingSphereRadius;
            CModel.Model.Tag = bs;
        }

        public override void OnCollision(Core.Player player)
        {
            Costam.collectEffect.Play();
            player.score += 1;

        }

        public override void OnCollision(Bullet bullet)
        {

        }
    }
}
