using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Core.Trigger_System
{
    public class AreaTrigger : Trigger, IPositionObject
    {
        /// <summary>
        /// Pozycja środka obszaru
        /// </summary>
        public Vector3 Position { get; set; }

        public float DimX { get; set; }
        public float DimY { get; set; }
        public float DimZ { get; set; }

        /// <summary>
        /// Obiekt, którego pozycję śledzimy
        /// </summary>
        public IPositionObject TrackedObject { get; set; }



        public AreaTrigger()
            : base()
        {

        }

        public AreaTrigger(Vector3 areaPosition,
            float dimX,
            float dimY,
            float dimZ,
            IPositionObject trackedObject)
            : base()
        {
            DimX = dimX;
            DimY = dimY;
            DimZ = dimZ;
            Position = areaPosition;
            TrackedObject = trackedObject;

            Condition = delegate()
            {
                return (
                    trackedObject.Position.X > Position.X - 1 / 2 * DimX &&
                    trackedObject.Position.X < Position.X + 1 / 2 * DimX &&
                     trackedObject.Position.Y > Position.Y - 1 / 2 * DimY &&
                    trackedObject.Position.Y < Position.Y + 1 / 2 * DimY &&
                     trackedObject.Position.Z > Position.Z - 1 / 2 * DimZ &&
                    trackedObject.Position.Z < Position.Z + 1 / 2 * DimZ
                    );
            };


        }



    }
}

