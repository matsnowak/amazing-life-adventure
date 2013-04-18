using LifeFall.Logic.Blood;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeFall.Logic.Sequences.Base;

namespace LifeFall.Logic.Sequences
{
    /// <summary>
    /// Pierścień. Elementy rozłożone równomiernie wokół osi tunelu
    /// </summary>
    /// <typeparam name="T"> Typ elementów jakie zawiera pierśnień</typeparam>
    public class CustomRing<T> : Sequence where T : MovableObject, new()
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="numberOfElements"> ilość elementów</param>
        /// <param name="radius"> odległość elementów od osi tunelu</param>
        public CustomRing(uint numberOfElements, float radius, float startAngle, float gapAngle)
            : base()
        {

            for (int i = 0; i < numberOfElements; ++i)
            {

                T element = Costam.MemoryPoolObjectProvider.GetObject<T>();
                element.Angle = startAngle + gapAngle * i;
                element.Radius = radius;
                AddObject(element);
            }
        }

        public CustomRing(List<int> elements, float radius, float startAngle, float gapAngle)
            : base()
        {
            foreach(int i in elements)
            {
                if (i != 0)
                {
                    T element = Costam.MemoryPoolObjectProvider.GetObject<T>();
                    element.Angle = startAngle + gapAngle * i;
                    element.Radius = radius;
                    AddObject(element);
                }
            }
        }


    }
}