using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeFall.Core
{
    public class TunnelPath
    {
        private int Length;
        private int Tessellation;
        private int Interval;
        private int Radius;
        private Random Random;

        private List<Vector3> controlPoints;
        private List<Vector3> pathPoints;

        public int Index
        { 
            get
            {
                if (controlPoints == null)
                    return 0;
                return controlPoints.Count() - 1;
            }
        }

        public TunnelPath(int length = 101, int tessellation = 15, int interval = 15, int radius = 40)
        {
            Tessellation = tessellation;
            Length = length;
            Interval = interval;
            Radius = radius;

            Random = new Random();

            controlPoints = new List<Vector3>();

            pathPoints = new List<Vector3>();

            Initialize();
        }

        private int InitLength
        {
            get
            {
                return 6; //TODO: uzależnić od rozdielczości jakoś i odległości jaką musimy widzieć itp....
            }
        }

        private void Initialize()
        {           
            //starting point
            controlPoints.Add(new Vector3(0, 0, 0));
            //must have 4 vectors to perform CatmullRom
            AddVector();
            AddVector();
            AddVector();

            Update();

            //other points to initlength
            AddSegment(Length - 1);
        }

        private void AddSegment()
        {
            AddVector();

            Update();
        }

        private void AddVector()
        {
            controlPoints.Add(new Vector3(controlPoints.ElementAt(Index).X + Interval, controlPoints.ElementAt(Index).Y + Random.Next(-Radius, 0), controlPoints.ElementAt(Index).Z + Random.Next(-Radius, Radius)));
        }

        public void AddSegment(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                AddSegment();
            }
        }

        public void DeleteSegment(int count = 1)
        {
            if (count < 0)
            {
                return;
            }
            if (count * Tessellation > pathPoints.Count)
            {
                pathPoints.Clear();
                return;
            }
            pathPoints.RemoveRange(0, count * Tessellation);
           
        }

        private void Update()
        {
            Vector3 p0 = controlPoints.ElementAt(Index - 3);
            Vector3 p1 = controlPoints.ElementAt(Index - 2);
            Vector3 p2 = controlPoints.ElementAt(Index - 1);
            Vector3 p3 = controlPoints.ElementAt(Index);

            pathPoints.AddRange(InterpolateCatmullRom(p0, p1, p2, p3));
        }

        public List<Vector3> PathPoints
        {
            get
            {
                //if (pathPoints == null)
                //{
                //    pathPoints = new List<Vector3>();
                //    for (int i = 0; i < ControlPoints.Count - 3; i += 1)
                //    {
                //        Vector3 p0 = ControlPoints.ElementAt(i);
                //        Vector3 p1 = ControlPoints.ElementAt(i + 1);
                //        Vector3 p2 = ControlPoints.ElementAt(i + 2);
                //        Vector3 p3 = ControlPoints.ElementAt(i + 3);

                //        pathPoints.AddRange(InterpolateCatmullRom(p0, p1, p2, p3));
                //    }
                //}
                return pathPoints;
            }
        }

        private List<Vector3> ControlPoints
        {
            get
            {
                //if (controlPoints == null)
                //{
                //    controlPoints = new List<Vector3>();
                //    controlPoints.Add(new Vector3(0, 0, 0));

                //    for (int i = 1; i < Length; i++)
                //    {
                //        //controlPoints.Add(new Vector3(controlPoints.ElementAt(i - 1).X + Interval, controlPoints.ElementAt(i - 1).Y + random.Next(-Radius, 0), controlPoints.ElementAt(i - 1).Z + random.Next(-Radius, Radius)));
                //        AddVector(i - 1);
                //        ////sinusoida
                //        //if (i % 2 != 0)
                //        //{
                //        //    ControlPoints.Add(new Vector3(ControlPoints.ElementAt(i - 1).X + Interval, ControlPoints.ElementAt(i - 1).Y + Radius, ControlPoints.ElementAt(i - 1).Z));
                //        //}
                //        //else
                //        //{
                //        //    ControlPoints.Add(new Vector3(ControlPoints.ElementAt(i - 1).X + Interval, ControlPoints.ElementAt(i - 2).Y, ControlPoints.ElementAt(i - 1).Z));
                //        //}
                //    }
                //}
                return controlPoints;
            }
        }

        #region CatmullRom

        private Vector3 CatmullRom(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float tessellation)
        {
            Vector3 result = new Vector3();
            result.X = MathHelper.CatmullRom(v1.X, v2.X, v3.X, v4.X, tessellation);
            result.Y = MathHelper.CatmullRom(v1.Y, v2.Y, v3.Y, v4.Y, tessellation);
            result.Z = MathHelper.CatmullRom(v1.Z, v2.Z, v3.Z, v4.Z, tessellation);
            return result;
        }

        private List<Vector3> InterpolateCatmullRom(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            List<Vector3> list = new List<Vector3>();
            int detail = Tessellation;
            for (int i = 0; i < detail; i++)
            {
                Vector3 newPoint = CatmullRom(v1, v2, v3, v4, (float)i / (float)detail);
                list.Add(newPoint);
            }
            return list;
        }

        #endregion CatmullRom

        #region Drawing Methods

        public void DrawTunnelPath(GraphicsDevice graphicsDevice)
        {
            if (PathPoints.Count > 0)
            {
                VertexPositionColor[] vectors = new VertexPositionColor[PathPoints.Count];

                for (int i = 0; i < PathPoints.Count; i++)
                {
                    if (i % 2 == 0)
                        vectors[i] = new VertexPositionColor(PathPoints.ElementAt(i), Color.Red);
                    else
                        vectors[i] = new VertexPositionColor(PathPoints.ElementAt(i), Color.Gold);
                }
                graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vectors, 0, vectors.Count() - 1);
            }
        }

        private void DrawLine(GraphicsDevice graphicsDevice, Vector3 point1, Vector3 point2)
        {
            VertexPositionColor[] vectors = new VertexPositionColor[2];

            vectors[0] = new VertexPositionColor(point1, Color.Red);
            vectors[1] = new VertexPositionColor(point2, Color.Gold);

            graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vectors, 0, 1);
        }

        private void DrawLine(GraphicsDevice graphicsDevice, List<Vector3> vectorsList)
        {
            if (vectorsList.Count > 0)
            {
                VertexPositionColor[] vectors = new VertexPositionColor[vectorsList.Count];

                for (int i = 0; i < vectorsList.Count; i++)
                {
                    if (i % 2 == 0)
                        vectors[i] = new VertexPositionColor(vectorsList.ElementAt(i), Color.White);
                    else
                        vectors[i] = new VertexPositionColor(vectorsList.ElementAt(i), Color.Black);
                }
                graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vectors, 0, vectors.Count() - 1);
            }
        }

        #endregion Drawing Methods

    }
}
