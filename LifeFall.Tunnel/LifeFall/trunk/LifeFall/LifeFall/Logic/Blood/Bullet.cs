using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic.Blood
{
    public class Bullet:  MovableObject
    {
        MovableObject Source { get; set; }
       
        Vector3 vector {get; set;}
        public static float speed = 1000;
        public static float distance = 500;
        float distanceCounter;

        public Bullet():base(Costam.BulletModel)
        {
            distanceCounter = 0;
            
            BoundingSphere bs = (BoundingSphere)CModel.Model.Tag;
            bs.Radius = Costam.BulletBoundingSphereRadius;
            CModel.Model.Tag = bs;
            CModel.textureEnabled = false;

        }

        public void Fire(MovableObject source, Vector3 target)
        {
            distanceCounter = 0;
            TunnelPosition = source.TunnelPosition;
            Angle = source.Angle;
            Radius = source.Radius;
            vector = target;

        }
        public override void OnCollision(Core.Player player)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            float delta = (float) gameTime.ElapsedGameTime.TotalSeconds * speed;
            distanceCounter += delta;
            Move(delta);
            if (distanceCounter >= distance)
            {
                DestroyBullet();
            }

        }

        public void DestroyBullet()
        {
            Costam.ObjectManager.MarkToDispose(this);
        }

        public override void OnCollision(Bullet b)
        {
            
        }


    }
}
