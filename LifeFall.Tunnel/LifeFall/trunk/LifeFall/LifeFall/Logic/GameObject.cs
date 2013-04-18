//#define DRAW_DEBUG_VECTORS
//#define DRAW_DEBUG_RIGID_BODY
using LifeFall.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.LinearMath;
using LifeFall.Debug;

namespace LifeFall.Logic
{
    public class GameObject
    {
        #region Properties
        static IndexFinder indexFinder = new IndexFinder();
        // System współrzędnych cylindrycznych

        public Vector3 TunnelPosition
        {
            get
            {
                return this.tunnelPosition;
            }
            set
            {
                tunnelPosition = value;
                CalculateNextPoints();
                CalculatePosition();
                CalculateRotations();
            }
        }
        protected Vector3 tunnelPosition;

        public float Angle { get { return angle; } set { angle = value; CalculatePosition(); CalculateRotations(); } }
        private float angle;

        public float Radius { get { return radius; } set { radius = value; if (radius == 0) radius = 0.1f;  CalculatePosition(); } }
        private float radius;

        public virtual Vector3 Position
        {
            get
            {
                return CModel.Position;
            }

            // Użycie tylko na własną odpowiedzialność
            set
            {
                CModel.Position = value;
            }
        }

        public Vector3 Forward { get; set; }
        public Vector3 UpVector { get; set; }
        public Vector3 Right { get; set; }

        public bool AutoRotate { get; set; }

        public CModel CModel;

        public List<Vector3> Track;

        protected int nextPointIndex;         // Indeks punktu w "track" leżacego bezpośrednio przed obiektem
        protected int previousPointIndex;     // Indeks punktu w "track" leżacego bezpośrednio za obiektem

        protected Vector3 backwardVector;

        #endregion Properties

        #region Methods

        public GameObject(Model model)
        {
            
            CModel = new CModel(model, Costam.Game.GraphicsDevice);
            Track = Costam.TunnelManager.tp.PathPoints;
            Radius = 0.1f;
            TunnelPosition = Track.ElementAt(0);
            AutoRotate = true;
            
            
        }

        public GameObject(Model model, GraphicsDevice graphicsDevice)
        {
            CModel = new CModel(model, graphicsDevice);
            Track = Costam.TunnelManager.tp.PathPoints;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public void SetProperties(Vector3 tunnelPosition, float angle, float radius)
        {
            this.tunnelPosition = tunnelPosition;
            this.angle = angle;
            this.Radius = radius;

            CalculateNextPoints();
            CalculatePosition();
            CalculateRotations();
        }

        protected void CalculateNextPoints()
        {
            nextPointIndex = Track.BinarySearch(tunnelPosition, indexFinder);

            // Jeśli BinarySearch zwrócił index dodatni tzn ze szukany punkt istnieje i nextPointIndex ma wskazywać następny
            if (nextPointIndex >= 0)
            {
                nextPointIndex++;
                previousPointIndex = nextPointIndex - 2;
            }
            else // Jeśli BiarySearch zwrócił wartość ujemną tzn że trzeba ją odwrócić i to będzie właściwy nextPointIndex
            {
                nextPointIndex *= -1;
                nextPointIndex--;
                previousPointIndex = nextPointIndex - 1;
            }

        }
        protected void CalculatePosition()
        {
            Vector3 forward = Vector3.Normalize(getNextPoint() - TunnelPosition);
            Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.Up, forward));
            Vector3 upVector = Vector3.Normalize(Vector3.Cross(forward, right));


            // Wydłużamy wektor i obracamy do okoła osi
            upVector *= Radius;
            upVector = Vector3.Transform(upVector, Matrix.CreateFromAxisAngle(forward, Angle));
            //right = Vector3.Transform(right, Matrix.CreateFromAxisAngle(forward, Angle));
            right = Vector3.Normalize(Vector3.Cross(upVector, forward));


            // koniec upVectora wskazuje punkt docelowy dla obiektu
            CModel.Position = tunnelPosition + upVector;
            CModel.PhysicsModel.RigidBody.Position = new JVector(CModel.Position.X, CModel.Position.Y, CModel.Position.Z);

            // Odwracamy żeby wskazywał oś tunelu
            UpVector = -1 * upVector;
            Forward = forward;
            Right = right;
        }

        protected void CalculateRotations()
        {
            Matrix rm = Matrix.Identity;

            if (true == AutoRotate)
            {
                rm.Up = Vector3.Normalize(UpVector);

                // TODO : WTF?? Zagadka dla Artura dlaczego musiałem zamienić te dwa wektory?  Zajrzeć do CalculatePosition
                rm.Forward = Vector3.Normalize(Right);
                rm.Right = Vector3.Normalize(Forward);
            }
            CModel.RotationMatrix = rm;
        }

        protected Vector3 getPoint(int index)
        {
            return Track.ElementAt(index);
        }

        public Vector3 getNextPoint()
        {
            return Track.ElementAt(nextPointIndex);
        }

        protected Vector3 getPreviousPoint()
        {
            //CheckRange(0, 1200); // TODO : Zmodyfikować jakby się zmieniła długość tunelu
            return Track.ElementAt(previousPointIndex);

        }

        void CheckRange(int min, int max)
        {
            if (previousPointIndex < min)
                previousPointIndex = min;
            if (previousPointIndex > max)
                previousPointIndex = max - 1;
            if (nextPointIndex < min)
                nextPointIndex = min + 1;
            if (nextPointIndex > max)
                nextPointIndex = max;
        }

        #endregion Methods

        #region Drawing

        public virtual void Draw()
        {
            ICamera cam = Costam.CameraManager.GetActiveCamera();
#if (DRAW_DEBUG_VECTORS && DEBUG)
               
                // Draw Forward
            Vector3 scaledForward = Forward * 50;

                Costam.DebugDraw.DrawLine(Position, Color.Red, Position + scaledForward ,Color.White);
                Costam.DebugDraw.DrawLine(TunnelPosition, Color.Red, TunnelPosition + scaledForward, Color.White);

                // Draw UpVector
                Costam.DebugDraw.DrawLine(Position, Color.Green, Position + UpVector, Color.White);

            //Draw Right
                Vector3 scaledRight = Right * 50;
                Costam.DebugDraw.DrawLine(Position, Color.Blue, Position + scaledRight, Color.White);

#endif

#if (DRAW_DEBUG_RIGID_BODY && DEBUG)
            //CModel.PhysicsModel.RigidBody.EnableDebugDraw = true;
            //CModel.PhysicsModel.RigidBody.DebugDraw(Costam.DebugDraw);
           

#endif

            CModel.Draw(cam.ViewMatrix, cam.ProjectionMatrix);
           
        }

        #endregion Drawing

        #region CoreStuff

        /// <summary>
        /// Comparator do sortowania vectorów po współrzędnych X
        /// </summary>
        public class IndexFinder : IComparer<Vector3>
        {

            public int Compare(Vector3 x, Vector3 y)
            {
                if (x.X < y.X)
                {
                    return -1;
                }
                else if (x.X > y.X)
                {
                    return 1;
                }
                return 0;
            }
        }
        #endregion CoreStuff
    }
}
