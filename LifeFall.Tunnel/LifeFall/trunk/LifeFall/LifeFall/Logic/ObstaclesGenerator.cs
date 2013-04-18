using LifeFall.Logic.Blood;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic
{
    public class ObstaclesGenerator<T> where T : MovableObject, new()
    {
        public MovableObject TrackedObject { get { return trackedObject; } set { trackedObject = value; lastObject.TunnelPosition = trackedObject.TunnelPosition; moveEverything(null); } }
        private MovableObject trackedObject;

        public int Density { get; set; }
        public float DistanceToTrackedObject { get; set; }
        public float PartLength { get; set; }
        public float MinRadius { get; set; }
        public float MaxRadius { get; set; }
        public float MaxAngle { get; set; }
        public float MinAngle { get; set; }

        public List<Behaviour> ObjectBehaviours;

        public bool Enabled { get; set; }

        private Sentinel sentinel;
        private List<float> positions;
        private List<MovableObject> obstacles;
        private MovableObject lastObject;

        public bool IsReady { get; private set; }

        public ObstaclesGenerator()
        {
            sentinel = new Sentinel();
            positions = new List<float>();
            obstacles = new List<MovableObject>();
            ObjectBehaviours = new List<Behaviour>();
            lastObject = new Sentinel();
        }

        public void Update(GameTime gameTime)
        {
            if (true == Enabled)
            {
                moveEverything(gameTime);
                if (lastObject.TunnelPosition.X < sentinel.TunnelPosition.X)
                {
                    IsReady = true;
                }
            }
        }

        public List<MovableObject> GetElements()
        {
            obstacles.Clear();
            if (true == IsReady && true == Enabled)
            {
                initPart();
                initOnPosition(sentinel.TunnelPosition);
                IsReady = false;
            }
            return obstacles;
        }
        private void initOnPosition(Vector3 position)
        {
            for (int i = 0; i < Density; ++i)
            {
                T obj = Costam.MemoryPoolObjectProvider.GetObject<T>();
                obj.TunnelPosition = position;
                obj.Move(positions[i]);
                obj.Speed = 0;
                obj.ClearBehaviours();
                foreach (Behaviour b in ObjectBehaviours)
                {
                    Behaviour nb = b.GetCopy();
                    nb.Init(obj);
                    obj.AddBehaviour(nb);
                }
                
                obj.Radius = (float)Costam.Random.NextDouble() * (MaxRadius - MinRadius) + MinRadius;
                obj.Angle = (float)Costam.Random.NextDouble() * (MaxAngle - MinAngle) + MinAngle;
                obstacles.Add(obj);
                lastObject = obj;
            }
        }

        private void initPart()
        {
            positions.Clear();
            for (int i = 0; i < Density; ++i)
            {
                float newPos = (float)Costam.Random.NextDouble() * PartLength;
                positions.Add(newPos);
            }
            positions.Sort();
        }

        private void moveEverything(GameTime gameTime)
        {
            sentinel.TunnelPosition = TrackedObject.TunnelPosition;
            sentinel.Move(DistanceToTrackedObject);
        }

    }
}
