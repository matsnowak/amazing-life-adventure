using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic.Behaviours
{
    public class BloodStream: Behaviour
    {
        const float maxRotationDelta = MathHelper.PiOver4;
        float rotationX = 0;
        float rotationY = 0;
        float rotationZ = 0;
        float rotationDelta = 0;

       

        public BloodStream()
        {
        }

        public override Behaviour GetCopy()
        {
            return new BloodStream();
        }

        public override void Init(MovableObject owner)
        {
            base.Init(owner);
            Execute = delegate(GameTime gameTime)
            {
                // Rotacja lokalna
                Vector3 oldRotation = owner.CModel.Rotation;
                rotationDelta = rotationX * (float)gameTime.ElapsedGameTime.TotalSeconds;
                oldRotation.X += rotationDelta;
                rotationDelta = rotationY * (float)gameTime.ElapsedGameTime.TotalSeconds;
                oldRotation.Y += rotationDelta;
                rotationDelta = rotationZ * (float)gameTime.ElapsedGameTime.TotalSeconds;
                oldRotation.Z += rotationDelta;
                owner.CModel.Rotation = oldRotation;

                // Zmiana prędkości krwi przy skurczu
                owner.Speed = Costam.BLOOD_SPEED;
            };

            Owner.AutoRotate = false;
            rotationX = (float) (Costam.Random.NextDouble() * maxRotationDelta * 2.0f) - maxRotationDelta ;
            rotationY = (float)(Costam.Random.NextDouble() * maxRotationDelta * 2.0f) - maxRotationDelta;
            rotationZ = (float)(Costam.Random.NextDouble() * maxRotationDelta * 2.0f) - maxRotationDelta;

        }
    }
}
