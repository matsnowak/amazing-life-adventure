using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LifeFall.Logic.Blood
{
    public class RedBloodCell : MovableObject
    {

        public RedBloodCell():base(Costam.BloodCellModel)
        {
            this.CModel.textureEnabled = true;
            this.CModel.texture = Costam.BloodCellTexture;
            BoundingSphere bs = (BoundingSphere)CModel.Model.Tag;
            bs.Radius = Costam.BloodCellBoundingSphereRadius;
            CModel.Model.Tag = bs;
           
        }


        public override void OnCollision(Core.Player player)
        {
            Costam.collisionEffect.Play();
            player.health -= 1;
            player.EnableGodMode();
        }

        public override void OnCollision(Bullet bullet)
        {
            Costam.PlayerManager.players.Values.ElementAt(0).score -= 2;
            if (Costam.PlayerManager.players.Values.ElementAt(0).score <= 0)
                Costam.PlayerManager.players.Values.ElementAt(0).score = 0;
        }
    }
}
