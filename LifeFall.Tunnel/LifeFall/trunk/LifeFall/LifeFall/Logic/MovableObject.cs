using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeFall.Core;
using LifeFall.Logic.Blood;

namespace LifeFall.Logic
{
    public abstract class MovableObject : GameObject
    {
        public MovableObject(Model model)
            : base(model)
        {
            Speed = 0;
            Behaviours = new List<Behaviour>();
        }


        List<Behaviour> Behaviours;
        public float Speed { get; set; }

        public void Move(float distance)
        {
            CalculateNextPoints();

            if (distance > 0)
            {
                float distanceToNextPoint = (getNextPoint() - TunnelPosition).Length();
                while (distance > distanceToNextPoint)
                {
                    distance -= distanceToNextPoint;

                    tunnelPosition = getNextPoint();
                    ++nextPointIndex;
                    distanceToNextPoint = (getNextPoint() - TunnelPosition).Length();
                }
                CalculateNextPoints();

                // Prosta interpolacja wektora
                Vector3 forward = getNextPoint() - TunnelPosition;
                forward.Normalize();
                forward *= distance;
                TunnelPosition += forward;

            }
            else if (distance < 0)
            { // poruszanie do tyłu, mało ważne ale działa
                distance *= -1;

                float distanceToPrevPoint = (getPreviousPoint() - TunnelPosition).Length();
                while (distance > distanceToPrevPoint)
                {
                    distance -= distanceToPrevPoint;
                    tunnelPosition = getPreviousPoint();
                    --previousPointIndex;
                    distanceToPrevPoint = (getPreviousPoint() - TunnelPosition).Length();
                }
                CalculateNextPoints();
                backwardVector = getPreviousPoint() - TunnelPosition;
                backwardVector.Normalize();
                backwardVector *= distance;
                Forward = -backwardVector;
                TunnelPosition += backwardVector;
            }

        }

        public override void Update(GameTime gameTime)
        {
            // TODO : A może zmieniona kolejność?
            float distance = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Move(distance);

            for (int i = 0; i < Behaviours.Count; ++i)
            {
                Behaviour b = Behaviours[i];
                if (b != null)
                {
                    b.Execute(gameTime);
                }
            }
            //foreach (Behaviour b in Behaviours)
            //{
            //    b.Execute(gameTime);
            //}

            base.Update(gameTime);
        }

        public void AddBehaviour(Behaviour behaviour)
        {
            if (!Behaviours.Contains(behaviour))
            {
                Behaviours.Add(behaviour);
            }
        }

        public void RemoveBahaviour(Behaviour behaviour)
        {
            Behaviours.Remove(behaviour);
        }

        public void ClearBehaviours()
        {
            Behaviours.Clear();
        }

        public abstract void OnCollision(Player player);
        public abstract void OnCollision(Bullet bullet);

    }
}
