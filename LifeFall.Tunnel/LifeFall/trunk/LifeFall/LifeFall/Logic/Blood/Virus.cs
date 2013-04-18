using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LifeFall.Logic.Blood
{
    public class Virus : MovableObject
    {
        public Virus():base(Costam.VirusModel)
        {
            BoundingSphere bs = (BoundingSphere)CModel.Model.Tag;
            bs.Radius = Costam.VirusBoundingSphereRadius;
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
            Costam.PlayerManager.players.Values.ElementAt(0).score += 5;
        }
    }
}
