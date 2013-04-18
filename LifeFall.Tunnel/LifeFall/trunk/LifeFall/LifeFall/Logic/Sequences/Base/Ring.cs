using LifeFall.Logic.Blood;

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic.Sequences.Base
{
    /// <summary>
    /// Pierścień. Elementy rozłożone równomiernie wokół osi tunelu
    /// </summary>
    /// <typeparam name="T"> Typ elementów jakie zawiera pierśnień</typeparam>
    public class Ring<T> : FastSequence where T : MovableObject, new()
    {
        uint defaultNumberOfElements;
        uint currentNumberOfElements;
        float radius;
        float angleOffset;
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="numberOfElements"> ilość elementów</param>
        /// <param name="radius"> odległość elementów od osi tunelu</param>
        public Ring(uint numberOfElements, float radius)
            : base()
        {
            this.currentNumberOfElements = this.defaultNumberOfElements = numberOfElements;
            this.radius = radius;
            this.angleOffset = MathHelper.TwoPi / numberOfElements;

        }

        public void EraseElement(float startAngle, float endAngle)
        {
            List<MovableObject> toRemove = new List<MovableObject>();
            foreach (MovableObject mo in objects)
            {
                if (mo.Angle > startAngle && mo.Angle < endAngle)
                {
                    toRemove.Add(mo);
                }
            }

        }

        public void EraseElements(int index, int count)
        {
            objects.RemoveRange(index, count);
        }

        public static List<T> GetRingOnPosition<T>(float numberOfElements, float radius, float startingAngle, Vector3 Position) where T: MovableObject, new()
        {
            List<T> staticObjectList = new List<T>();
            float delatAngle = MathHelper.TwoPi / numberOfElements;
            for (int i = 0; i < numberOfElements; ++i)
            {
                T element = Costam.MemoryPoolObjectProvider.GetObject<T>();
                element.TunnelPosition = Position;
                element.Angle = i * delatAngle + startingAngle;
                element.Radius = radius;
                staticObjectList.Add(element);
            }
            return staticObjectList;
        }

        public override void ConstructOnPosition(Vector3 position)
        {
            Reset();
            objects.AddRange(GetRingOnPosition<T>(currentNumberOfElements, radius, Angle, position));
        }

        public override void BalanceDifficulty(float ratio)
        {
            currentNumberOfElements = (uint) (defaultNumberOfElements * ratio);
        }

        public override float GetCurrentLength()
        {
            return 10;  // TODO: może inna wartość? np szerokość obiektów jakie zabiera
           
        }



    }
}
