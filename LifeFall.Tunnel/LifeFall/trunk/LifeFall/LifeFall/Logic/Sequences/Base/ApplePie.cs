using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LifeFall.Logic.Sequences.Base
{
    class ApplePie<T> : Sequence where T : MovableObject, new()
    {
        public ApplePie(float gapStartAngle)
            : base()
        {

            AddSequence(new CustomRing<T>(55, 90, gapStartAngle, 0.1f));
            AddSequence(new CustomRing<T>(55, 80, gapStartAngle, 0.1f));
            AddSequence(new CustomRing<T>(55, 70, gapStartAngle, 0.1f));
            AddSequence(new CustomRing<T>(55, 60, gapStartAngle, 0.1f));
            AddSequence(new CustomRing<T>(55, 50, gapStartAngle, 0.1f));
            AddSequence(new CustomRing<T>(55, 40, gapStartAngle, 0.1f));
            AddSequence(new CustomRing<T>(55, 30, gapStartAngle, 0.1f));
            AddSequence(new CustomRing<T>(55, 20, gapStartAngle, 0.1f));

        }

    }
}
