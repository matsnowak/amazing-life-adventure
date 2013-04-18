using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeFall.Logic.Sequences.Base;

namespace LifeFall.Logic.Sequences.Custom
{
    class MultipleApplePies<T> : Sequence where T : MovableObject, new()
    {
        public MultipleApplePies(uint numberOfElements,uint elementsOffset, float startAngle, float angleOffset)
            : base()
        {
            for (int i = 0; i < numberOfElements; i++)
            {
                ApplePie<T> r = new ApplePie<T>(startAngle + i * angleOffset);
                r.Offset = i * elementsOffset;
                AddSequence(r);
            }
        }

    }
}
