using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace LifeFall.Core
{
    public class Tunnel : DrawablePrimitive
    {
        #region Fields

        short tessellation;
        int segmentsCount;
        float textureScale = Costam.TUNNEL_TEXTURE_SCALE_X;
        List<Vector3> axisPoints;

        VertexBuffer[] vertexBuffers;
        int currentVertexBuffer;
        int vertexBufferSize;

        public int radius;

        #endregion


        public Tunnel(GraphicsDevice graphicsDevice, short tesselation, List<Vector3> TunnelPointsSource, int radius)
            : base(graphicsDevice)
        {
            this.tessellation = tesselation;
            segmentsCount = 0;

            axisPoints = TunnelPointsSource;
            this.radius = radius;

            GenerateTunnellFromSource();
            vertexBufferSize = vertices.Count;

            vertexBuffers = new VertexBuffer[2];
            vertexBuffers[0] = new VertexBuffer(graphicsDevice,
                typeof(VertexPositionNormalTexture),
                vertexBufferSize,
                BufferUsage.None);

            vertexBuffers[1] = new VertexBuffer(graphicsDevice,
                typeof(VertexPositionNormalTexture),
                vertexBufferSize,
                BufferUsage.None);
            currentVertexBuffer = 0;

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort),
                                             indices.Count, BufferUsage.None);

            SetVertexBuffer(currentVertexBuffer);
            SetIndexBuffer();
            SetBasicEffect();
        }

        public void StartDrawingNextPart()
        {
            currentVertexBuffer = (currentVertexBuffer + 1) % 2;
            SetVertexBuffer(currentVertexBuffer);
        }
        void SetVertexBuffer(int VertexBufferNumber)
        {
            vertexBuffers[VertexBufferNumber].SetData(vertices.GetRange(0, vertexBufferSize).ToArray());
        }
        void SetIndexBuffer()
        {
            indexBuffer.SetData(indices.ToArray());
        }
        void SetBasicEffect()
        {
            basicEffect = new BasicEffect(graphicsDevice);
        }

        /// <summary>
        /// Tworzy verteksy dla jednej z 4 części TunnelPointSource. Części numerowane od 0 do 3
        /// </summary>
        /// <param name="part"></param>
        public void InitVerticesPart(byte part)
        {
            if (part < 0 || part > 3)
                throw new ArgumentOutOfRangeException("part");

            int quarterLength = (int)axisPoints.Count() / 4;


            Vector3 tunelDirection;
            Matrix transformMatrix = Matrix.Identity;

            for (int i = 0; i < quarterLength; i++)
            {
                tunelDirection = axisPoints.ElementAt(i + 1 + part * quarterLength) - axisPoints.ElementAt(i + part * quarterLength);

                float yaw = (float)Math.Atan(-tunelDirection.Z / tunelDirection.X);
                float roll = (float)Math.Atan(tunelDirection.Y / tunelDirection.X);
                float pitch = 0;    // Niepotrzebne obracanie do okoła osi

                transformMatrix = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateTranslation(axisPoints.ElementAt(i + part * quarterLength));

                AddSegment(transformMatrix, radius);
            }
        }

        public void RemoveVerticesPart(byte part)
        {
            int quarterLength = (int)axisPoints.Count() / 4;
            vertices.RemoveRange(part * quarterLength * (tessellation + 1), quarterLength * (tessellation + 1));

        }

        void GenerateTunnellFromSource()
        {
            Vector3 tunelDirection;
            Matrix transformMatrix = Matrix.Identity;
            int size = (int)(3 * axisPoints.Count() / 4);
            for (int i = 0; i < size; i++)
            {
                tunelDirection = axisPoints.ElementAt(i + 1) - axisPoints.ElementAt(i);

                float yaw = (float)Math.Atan(-tunelDirection.Z / tunelDirection.X);
                float roll = (float)Math.Atan(tunelDirection.Y / tunelDirection.X);
                float pitch = 0;// (float)Math.Atan(tunelDirection.Y / tunelDirection.Z);

                transformMatrix = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateTranslation(axisPoints.ElementAt(i));
                AddSegmentWithInitIndices(transformMatrix, radius);
                
            }

        }

        public void AddSegmentWithInitIndices(Vector3 position, Matrix rotationMatrix, float radius)
        {
            AddSegmentWithInitIndices(rotationMatrix * Matrix.CreateTranslation(position), radius);
        }

        /// <summary>
        /// Dodaje segmenty i inicjalizuje wskaźniki
        /// </summary>
        /// <param name="transformMatrix"></param>
        /// <param name="radius"></param>
        public void AddSegmentWithInitIndices(Matrix transformMatrix, float radius)
        {
            int j = segmentsCount;

            for (int i = 0; i <= tessellation; i++)
            {
                Vector3 normal = GetCircleVector(i, tessellation);
                Vector3 pos = normal * radius;
                pos = Vector3.Transform(pos, transformMatrix);
                //normal = Vector3.Transform(normal, transformMatrix);
                normal.Normalize();

                AddVertex(pos, normal, new Vector2(textureScale * (float)pos.X / 256.0f, textureScale * (float)i / tessellation));
                if (j > 0)
                {
                    int firstVectorIndex = (j - 1) * (tessellation + 1) + i;
                    int secondVectorIndex = (j - 1) * (tessellation + 1) + (i + 1) % (tessellation + 1);
                    int thirdVectorIndex = j * (tessellation + 1) + i;

                    AddIndex(firstVectorIndex);
                    AddIndex(secondVectorIndex);
                    AddIndex(thirdVectorIndex);

                    //SetNormalToTriangle(vertices, firstVectorIndex, secondVectorIndex, thirdVectorIndex);


                    firstVectorIndex = (j - 1) * (tessellation + 1) + (i + 1) % (tessellation + 1);
                    secondVectorIndex = j * (tessellation + 1) + i;
                    thirdVectorIndex = j * (tessellation + 1) + (i + 1) % (tessellation + 1);

                    AddIndex(firstVectorIndex);
                    AddIndex(secondVectorIndex);
                    AddIndex(thirdVectorIndex);

                    //SetNormalToTriangle(vertices, firstVectorIndex, secondVectorIndex, thirdVectorIndex);
                }


            }

            segmentsCount++;
        }

        /// <summary>
        /// Dodaje segmenty i NIE inicjalizuje wskaźników. Zalecane jest używanie poprzedniego IndexBuffera
        /// </summary>
        /// <param name="transformMatrix"></param>
        /// <param name="radius"></param>
        public void AddSegment(Matrix transformMatrix, float radius)
        {
            for (int i = 0; i <= tessellation; i++)
            {
                Vector3 normal = GetCircleVector(i, tessellation);
                Vector3 pos = normal * radius;
                pos = Vector3.Transform(pos, transformMatrix);
                
                normal.Normalize();
                AddVertex(pos, normal, new Vector2(textureScale * (float)pos.X / 256.0f, textureScale * (float)i / tessellation));
            }

        }

        /// <summary>
        /// Helper method computes a point on a circle.
        /// </summary>
        static public Vector3 GetCircleVector(int i, int tessellation)
        {
            float angle = i * MathHelper.TwoPi / tessellation;

            float dy = (float)Math.Cos(angle);
            float dz = (float)Math.Sin(angle);

            return new Vector3(0, dy, dz);
        }


        #region Drawing
        public override void Draw(Effect effect)
        {
            graphicsDevice = effect.GraphicsDevice;

            graphicsDevice.SetVertexBuffer(vertexBuffers[currentVertexBuffer]);
            graphicsDevice.Indices = indexBuffer;

            foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();

                int primitiveCount = indices.Count / 3;
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                     vertices.Count, 0, primitiveCount);
            }
        }
        #endregion

        private Vector3 CalculateNormal(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            Vector3 firstVector = vertex2 - vertex1;
            Vector3 secondVector = vertex3 - vertex1;

            Vector3 normal = Vector3.Cross(firstVector, secondVector);
            return normal;
        }

        private void SetNormalToTriangle(List<VertexPositionNormalTexture> vertices,
                                            int firstVectorIndex,
                                            int secondVectorIndex,
                                            int thirdVectorIndex)
        {
            VertexPositionNormalTexture firstV = vertices.ElementAt(firstVectorIndex);
            VertexPositionNormalTexture secondV = vertices.ElementAt(secondVectorIndex);
            VertexPositionNormalTexture thirdV = vertices.ElementAt(thirdVectorIndex);

            Vector3 newNormal = CalculateNormal(firstV.Position, secondV.Position, thirdV.Position);

            firstV.Normal = newNormal;
            secondV.Normal = newNormal;
            thirdV.Normal = newNormal;

            vertices[firstVectorIndex] = firstV;
            vertices[secondVectorIndex] = secondV;
            vertices[thirdVectorIndex] = thirdV;
        }

        float normalLength = 15;
        public void DrawNormals()
        {

            for (int i = 0; i < vertices.Count; ++i)
            {
                Vector3 newNormal = vertices[i].Normal * normalLength;
                Costam.DebugDraw.DrawLine(vertices[i].Position, vertices[i].Position + newNormal);
            }
        }

    }

}
