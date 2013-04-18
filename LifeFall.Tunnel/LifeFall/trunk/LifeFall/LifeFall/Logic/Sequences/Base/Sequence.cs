using LifeFall.Logic.Blood;
using LifeFall.Logic.Sequences.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic.Sequences.Base
{

    /// <summary>
    /// Sekwencja przeszkód 
    /// </summary>
    public class Sequence
    {
        protected List<Sequence> children;
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

        /// <summary>
        /// Pprzesunięcie każdego elementu sekwencji względem TunnelPosition
        /// </summary>
        public float Offset
        {
            get { return offset; }
            set
            {
                deltaOffset = value - offset;
                offset = value;
                updateOffset();
            }
        }
        private float deltaOffset = 0;
        private float offset = 0;

        /// <summary>
        /// Pozycja w tunelu. Ustawia wszystkie obiekty o offset dalej od tego parametru. Ustawia taką samą wartość dla dzieci
        /// </summary>
        public Vector3 TunnelPosition 
        { 
            get { return tunnelPosition; }
            set 
            { 
                tunnelPosition = value;
                foreach (MovableObject mo in objects)
                {
                    mo.TunnelPosition = tunnelPosition;
                    mo.Move(offset);
                }

                foreach (Sequence s in children)
                {
                    s.TunnelPosition = tunnelPosition;
                }
            }
        }
        private Vector3 tunnelPosition = Vector3.Zero;


        /// <summary>
        /// Trudność pokonania sekwencji. 
        /// </summary>
        public float Difficulty
        { // TODO : Dobrać odpowiednie wyważenie
            get { return difficulty; }
            set { difficulty = value; }
        }
        private float difficulty = 0;


        #region Methods

        public Sequence()
        {
            children = new List<Sequence>();
            objects = new List<MovableObject>();
           
        }

        /// <summary>
        /// Wywołuje update dla wszystkich sekwencji dzieci.
        /// </summary>
        /// <param name="gameTime"> Czas klatki</param>
        public virtual void Update(GameTime gameTime)
        {
            foreach (Sequence s in children)
            {
                s.Update(gameTime);
            }
        }

        /// <summary>
        /// Dodaje sekwencje jako dziecko
        /// </summary>
        /// <param name="sequence"></param>
        public void AddSequence(Sequence sequence)
        {
            children.Add(sequence);
        }

        /// <summary>
        /// Usuwa sekwencje z listy dzieci
        /// </summary>
        /// <param name="sequence"></param>
        public void RemoveSequence(Sequence sequence)
        {
            children.Remove(sequence);
        }

        /// <summary>
        /// Dodaje obiekt do sekwencji
        /// </summary>
        /// <param name="movableObject"></param>
        public void AddObject(MovableObject movableObject)
        {
            objects.Add(movableObject);
        }

        /// <summary>
        /// Usuwa obiekt z sekwencji
        /// </summary>
        /// <param name="movableObject"></param>
        public void RemoveObject(MovableObject movableObject)
        {
            objects.Remove(movableObject);
        }

        /// <summary>
        /// Zwraca liste obiektów w sekwencji
        /// </summary>
        /// <returns>Lista obiektów w danej sekwencji łacznie z obiektami sekwencji dzieci</returns>
        public List<MovableObject> GetObjectList()
        {
            List<MovableObject> returnList = new List<MovableObject>();
            foreach (MovableObject mo in objects)
            {
                mo.Angle += deltaAngle;
                mo.TunnelPosition = TunnelPosition;
                mo.Move(Offset);

                returnList.Add(mo);
            }

            foreach (Sequence s in children)
            {
                returnList.AddRange(s.GetObjectList());
            }

            objects.Clear();            // TODO : Sprawdzić czy nie usuwa obiektów zamiast odpinać referencje
            return returnList;
        }

        /// <summary>
        ///  Konstruuje sekwencje i alokuje pamięć dla obiektów. Obiekty pobiera z memory poola
        /// </summary>
        public virtual void Construct()
        {
            foreach (Sequence s in children)
            {
                s.Construct();
            }
        }

        public List<MovableObject> GetObjectListOnPosition(Vector3 tunnelPosition)
        {
            TunnelPosition = tunnelPosition;
            return GetObjectList();
        }

        /// <summary>
        /// Zmienia trudność seksencji wykorzystująć ratio. 
        /// </summary>
        /// <param name="ratio">wsółczynnik zmian. Współczynnik równy 1 oznacza brak zmian </param>
        public virtual void BalanceDifficulty(float ratio)
        {
            for (int i = 0; i < children.Count; ++i)
            {
                children[i].BalanceDifficulty(ratio);
            }
        }

        public virtual float GetCurrentLength()
        {
            float totalLength = 0;
            for (int i = 0; i < children.Count; i++)
            {
                totalLength += children[i].GetCurrentLength();
            }
            return totalLength;

        }

        private void updateAngle()
        {
            foreach (MovableObject mo in objects)
            {
                mo.Angle += deltaAngle;
            }
            foreach (Sequence s in children)
            {
                s.Angle = angle;
            }
        }

        private void updateOffset()
        {
            foreach (MovableObject mo in objects)
            {
                mo.TunnelPosition = TunnelPosition;
                mo.Move(Offset);
            }
            foreach (Sequence s in children)
            {
                s.Offset += deltaOffset;
                int a = 2;
                a *= 5;
            }
        }

        #endregion Methods
    }
}
