using LifeFall.Logic.Sequences.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic.Sequences
{

    public enum TurningDirection { Left = -1, Right = 1 };



    /// <summary>
    /// Helisa.
    /// </summary>
    /// <typeparam name="T">Typ obiektów z których się składa</typeparam>
    public class Helix<T> : FastSequence where T : MovableObject, new()
    {
        public uint numberOfElements;
        public float totalAngle;
        public float defaultAngle;
        public float defaultLength;
        public float radius;
        public float Length;
        public TurningDirection turningDirection;
        public uint multiplicity;
        public uint defaultMulticiplity;

        float angleOffset;
        float positionOffset;


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="numberOfElements"> ilośc elementów z którch ma się składać pojedyncza helisa</param>
        /// <param name="totalAngle"> kąt o jaki ma się obrócić ostatni element</param>
        /// <param name="radius"> odległośc elementów od osi tunelu</param>
        /// <param name="Length"> dłogość helisy</param>
        /// <param name="turningDirection"> kierunek skrętu helisy</param>
        /// <param name="multiplicity"> krotność helisy</param>
        public Helix(
            uint numberOfElements,
            float totalAngle,
            float radius,
            float Length,
            TurningDirection turningDirection = TurningDirection.Right,
            uint multiplicity = 1
            )
            : base()
        {
            this.numberOfElements = numberOfElements;
            this.radius = radius;
            this.totalAngle = defaultAngle = totalAngle;
            this.Length = defaultLength = Length;
            this.turningDirection = turningDirection;
            this.multiplicity = defaultMulticiplity = multiplicity;

            this.angleOffset = totalAngle / numberOfElements;
            this.positionOffset = Length / numberOfElements;

        }

        List<T> ringList;
        public override void ConstructOnPosition(Vector3 position)
        {
            Reset();
            objects.AddRange(GetHelixOnPosition(numberOfElements, totalAngle, Angle, radius, Length, turningDirection, multiplicity, position));
        }

        public static List<T> GetHelixOnPosition(
            uint numberOfElements,
            float totalAngle,
            float startingAngle,
            float radius,
            float Length,
            TurningDirection turningDirection,
            uint multiplicity,
            Vector3 position
            )
        {
            List<T> returnList = new List<T>();
            float angleOffset = totalAngle / numberOfElements;
            float positionOffset = Length / numberOfElements;

            for (int i = 0; i < numberOfElements; ++i)
            {
                List<T> ringList = Ring<T>.GetRingOnPosition<T>(multiplicity, radius, angleOffset * i * (int)turningDirection + startingAngle, position);
                for (int j = 0; j < ringList.Count; ++j)
                {
                    ringList[j].Move(i * positionOffset);
                }
                returnList.AddRange(ringList);
            }
            return returnList;

        }

        public override void BalanceDifficulty(float ratio)
        {
            multiplicity = defaultMulticiplity + (uint)Math.Floor(ratio);
            float frac = ratio - (float)Math.Floor(ratio);
            totalAngle = defaultAngle * (1 + frac/2);
            Length = defaultLength * (1 - frac / 2);
        }

        public override float GetCurrentLength()
        {
            return Length;
        }
    }
}
