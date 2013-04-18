using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LifeFall.Core;
using Jitter;
using Jitter.Collision;


namespace LifeFall
{

    
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DemoChaseCamera : Microsoft.Xna.Framework.GameComponent, ICamera
    {
        public List<Vector3> track;

        int possitionPointNumber;
        int targetPointNumber;

        int nextPointNumber;
        int previousPointNumber;
        Vector3 nextPointVector;
        Vector3 previousPointVector;


        float speed = 0.00008f;

        public DemoChaseCamera(Game game)
            : base(game)
        {
            possitionPointNumber = 5;
            targetPointNumber = possitionPointNumber + 20;

            nextPointNumber = possitionPointNumber + 1;
            previousPointNumber = possitionPointNumber - 1;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            CreateLookAt();

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                1, 5000);



            base.Initialize();
        }

        public void InitCamerka()
        {
            Vector3 PositionPoint = track.ElementAt(possitionPointNumber);
            Vector3 targetPoint = track.ElementAt(targetPointNumber);

            nextPointVector = track.ElementAt(nextPointNumber);
            previousPointVector = track.ElementAt(previousPointNumber);

            Position = PositionPoint;
            Forward = targetPoint - PositionPoint;
            Forward.Normalize();
            UpVector = Vector3.Cross(Forward, Vector3.Right);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //nextPointNumber = track.FindIndex(new Predicate<Vector3>(delegate(Vector3 t) { return t.Equals(nextPointVector); }));
            nextPointNumber = track.FindIndex(w => (w.X == nextPointVector.X && w.Y == nextPointVector.Y && w.Z == nextPointVector.Z));
            
            targetPointNumber = nextPointNumber + 20;

            float distance = gameTime.ElapsedGameTime.Ticks * speed;

            float distanceToNextPoint = (getPoint(nextPointNumber) - Position).Length();
            while (distance > distanceToNextPoint)
            {
                distance -= distanceToNextPoint;
                Position = getPoint(nextPointNumber);
                nextPointNumber++;
                distanceToNextPoint = (getPoint(nextPointNumber) - Position).Length();
            }
            Vector3 directionVector = getPoint(nextPointNumber) - Position;
            directionVector.Normalize();
            directionVector *= distance;
            Position += directionVector;

            Vector3 forwardVector = getPoint(targetPointNumber) - Position;
            Forward = forwardVector;
            Forward.Normalize();
            nextPointVector = getPoint(nextPointNumber);

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up))
            {
                if (speed < 0.000150)
                {
                    speed += 0.000005f;
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                
                speed -= 0.000005f;
                if (speed < 0)
                    speed = 0;
            }
            //UpVector = Vector3.Cross(Forward, Vector3.Right);
            CreateLookAt();
            base.Update(gameTime);
        }

        private void CreateLookAt()
        {
            ViewMatrix = Matrix.CreateLookAt(Position,
                Position + Forward, UpVector);
        }

        private Vector3 getPoint(int number)
        {
            return track.ElementAt(number);
        }

        public Vector3 Forward
        {
            get;
            set;
        }

        public Vector3 UpVector
        {
            get;
            set;
        }

        public Matrix ViewMatrix
        {
            get;
            set;
        }

        public Matrix ProjectionMatrix
        {
            get;
            set;
        }

        public Vector3 Position
        {
            get;
            set;
        }
    }




}
