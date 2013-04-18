using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic.Sequences.Base
{
    public abstract class FastSequence
    {
      protected List<MovableObject> objects;
       
        /// <summary>
        /// Kąt obrotu każdego elementu sekwencji do okoła tunelu
        /// </summary>
        public float Angle
        {
            get { return angle; }
            set
            {
                deltaAngle = value - angle;
                angle = value;
                updateAngle();
            }
        }
        private float angle = 0;
        private float deltaAngle = 0;

        public float Difficulty
        { // TODO : Dobrać odpowiednie wyważenie
            get { return difficulty; }
            set { difficulty = value; }
        }
        private float difficulty = 0;


        #region Methods

        public FastSequence()
        {
            objects = new List<MovableObject>();
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void AddObject(MovableObject movableObject)
        {
            objects.Add(movableObject);
        }

        public void RemoveObject(MovableObject movableObject)
        {
            objects.Remove(movableObject);
        }

        public List<MovableObject> GetObjectList()
        {
            return objects;
        }

        /// <summary>
        ///  Konstruuje sekwencje na określonej pozycji i alokuje pamięć dla obiektów. Obiekty pobiera z memory poola
        /// </summary>
        public abstract void ConstructOnPosition(Vector3 position);
 


        /// <summary>
        /// Zmienia trudność seksencji wykorzystująć ratio. 
        /// </summary>
        /// <param name="ratio">wsółczynnik zmian. Współczynnik równy 1 oznacza brak zmian </param>
        public abstract void BalanceDifficulty(float ratio);


        /// <summary>
        /// Zwraca długość sekwencji
        /// </summary>
        /// <returns></returns>
        public abstract float GetCurrentLength();


        private void updateAngle()
        {
            foreach (MovableObject mo in objects)
            {
                mo.Angle += deltaAngle;
            }
        }

        /// <summary>
        /// Używać na początku każdego CreateOnPosition()
        /// </summary>
        protected void Reset()
        {
            objects.Clear();
        }

        #endregion Methods

    }
}
