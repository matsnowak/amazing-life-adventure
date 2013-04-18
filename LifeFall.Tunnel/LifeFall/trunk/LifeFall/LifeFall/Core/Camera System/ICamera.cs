using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Core
{
    public interface ICamera : IPositionObject
    {
         Vector3 Forward { get;  set; }
         Vector3 UpVector { get;  set; }

         Matrix ViewMatrix { get;  set; }
         Matrix ProjectionMatrix { get;  set; }
    }
}
