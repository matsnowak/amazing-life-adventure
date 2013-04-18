using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic.Blood
{
    public class Sentinel:  MovableObject
    {
        public Sentinel()
            : base(Costam.BloodCellModel) // TODO : Bron boze zeby to rysowac
        { }

        public override void Draw()
        {
            throw new InvalidOperationException("Object is not designed to draw");
        }
        public override void OnCollision(Core.Player player)
        {
            
        }

        public override void OnCollision(Bullet bullet)
        {

        }

    }
}
