using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeFall.Logic.Blood;

namespace LifeFall.Logic
{
    public class SimpleMemoryPool : IObjectsProvider
    {
        public const int DefaultVirusPoolSize = 100;
        public const int DefaultRedBloodCellPoolSize = 100;
        public const int DefaultCollectiblePointPoolSize = 100;

        public const int DefaultPoolSize = 100;

        public const float MinimumPoolSizePercentage = 20;
        public const int deltaSize = 0;

        public readonly Dictionary<Type, Queue<MovableObject>> poolContainter = new Dictionary<Type, Queue<MovableObject>>();
        private readonly Dictionary<Type, int> poolSizes = new Dictionary<Type, int>();
       
        #region Methods

        ///<summary>
        ///Konstruktor
        ///</summary>
        ///<param name="virusPoolSize"></param>
        ///<param name="redBloodCellPoolSize"></param>
        ///<param name="collectiblePointPoolSize"></param>
        public SimpleMemoryPool(
            int virusPoolSize = DefaultVirusPoolSize,
            int redBloodCellPoolSize = DefaultRedBloodCellPoolSize,
            int collectiblePointPoolSize = DefaultCollectiblePointPoolSize)
        {
            // Tutaj definiujemy jakie chcemy mieć memoryPoole, 
            // TYP, pojemność
            //poolSizes.Add(typeof(Virus), DefaultVirusPoolSize);
            //poolSizes.Add(typeof(RedBloodCell), DefaultRedBloodCellPoolSize);
            //poolSizes.Add(typeof(CollectiblePoint), DefaultCollectiblePointPoolSize);

            foreach (KeyValuePair<Type, int> pair in poolSizes)
            {
                Type type = pair.Key;
                int size = pair.Value;
                Queue<MovableObject> q = new Queue<MovableObject>();
                for (int i = 0; i < size; ++i)
                {
                    var obj = Activator.CreateInstance(Type.GetType(type.ToString()));
                    q.Enqueue(obj as MovableObject);
                }
                poolContainter.Add(type, q);
            }

        }

        public T GetObject<T>() where T : MovableObject, new()
        {
            GetObjectCounter++;
            Queue<MovableObject> objectsPool;
            poolContainter.TryGetValue(typeof(T), out objectsPool);
                //poolContainter[typeof(T)];
            
            if (objectsPool == null)
            { // nie ma takiego typu w liscie kolejek, wiec ja tworzymy
                Type type = typeof(T);
                int size = DefaultPoolSize;
                Queue<MovableObject> q = new Queue<MovableObject>();
                for (int i = 0; i < size; ++i)
                {
                    var obj = Activator.CreateInstance(Type.GetType(type.ToString()));
                    q.Enqueue(obj as MovableObject);
                }
                poolContainter.Add(type, q);
                poolSizes.Add(type, size);

                objectsPool = q;
            }

            MovableObject m = objectsPool.Dequeue();
            int expectedPoolSize = poolSizes[typeof(T)];
            checkPool<T>(objectsPool, expectedPoolSize);

            return (T)m;
        }

        /// <summary>
        /// Zwraca obiekt do odpowiedniej puli
        /// </summary>
        /// <param name="poolObject"></param>
        public void Dispose(MovableObject poolObject)
        {
            poolObject.ClearBehaviours();
            poolObject.Speed = 0;
            poolObject.AutoRotate = true;
            Queue<MovableObject> q = poolContainter[poolObject.GetType()];
            q.Enqueue(poolObject);

            DisposeCounter++;
        }

        /// <summary>
        /// Sprawdza czy rozmiar elementów puli nie jest mniejszy niż minimalny dopuszczalny rozmiar
        /// </summary>
        /// <typeparam name="T">Typ obiektów puli</typeparam>
        /// <param name="pool">pula</param>
        /// <param name="ExpectedSize"> oczekiwany rozmiar puli. Na jego podstawie jest obliczaly minimaly dopuszczalny rozmiar</param>
        void checkPool<T>(Queue<T> pool, int ExpectedSize) where T : MovableObject, new()
        {
            int minimumExpectedSize = (int)(ExpectedSize * MinimumPoolSizePercentage / 100);
            if (pool.Count < (minimumExpectedSize - deltaSize))
            {
                for (int i = 0; i < minimumExpectedSize; ++i)
                {
                    pool.Enqueue(new T());
                }
            }
        }

        void checkPool<T>(Queue<MovableObject> pool, int ExpectedSize) where T : MovableObject, new()
        {
            int minimumExpectedSize = (int)(ExpectedSize * MinimumPoolSizePercentage / 100);
            if (pool.Count < (minimumExpectedSize - deltaSize))
            {
                for (int i = 0; i < minimumExpectedSize; ++i)
                {
                    pool.Enqueue(new T());
                }
            }
        }

        #endregion Methods

        public int DisposeCounter = 0;
        public int GetObjectCounter = 0;
    }
}
