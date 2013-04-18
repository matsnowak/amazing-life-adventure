using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LifeFall.Core.Trigger_System
{

    /// <summary>
    /// Trigger wyzwalany po tym jak trackeObject.Position.X dotrze do punktu PositionX
    /// </summary>
    public class PositionXReachedTrigger : AreaTrigger
    {
        public PositionXReachedTrigger(IPositionObject trackedObject, float PositionX)
            : base()
        {
            Position = new Vector3(PositionX, 0, 0);
            TrackedObject = trackedObject;

            Condition = delegate()
            {
                return TrackedObject.Position.X > Position.X;
            };

        }
    }
}
