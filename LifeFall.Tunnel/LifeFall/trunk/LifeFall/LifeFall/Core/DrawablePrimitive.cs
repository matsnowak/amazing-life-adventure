using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeFall.Core
{
    public abstract class DrawablePrimitive
    {
 
            #region Fields

            protected List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
            protected List<ushort> indices = new List<ushort>();

            protected VertexBuffer vertexBuffer;
            protected IndexBuffer indexBuffer;
            protected BasicEffect basicEffect;

            protected GraphicsDevice graphicsDevice;

            #endregion

            #region Initialization

            protected DrawablePrimitive(GraphicsDevice graphicsDevice)
            {
                this.graphicsDevice = graphicsDevice;
            }
            protected void AddVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
            {
                vertices.Add(new VertexPositionNormalTexture(position, normal, textureCoordinate));
            }

            protected void AddIndex(int index)
            {
                //if (index > ushort.MaxValue)
                //    throw new ArgumentOutOfRangeException("index");

                indices.Add((ushort)index);
            }

            protected int CurrentVertex
            {
                get { return vertices.Count; }
            }

            protected void InitializePrimitive()
            {
                InitializePrimitive(graphicsDevice);
            }

            protected void InitializePrimitive(GraphicsDevice graphicsDevice)
            {
                vertexBuffer = new VertexBuffer(graphicsDevice,
                                                typeof(VertexPositionNormalTexture),
                                                vertices.Count, BufferUsage.None);
                vertexBuffer.SetData(vertices.ToArray());
                indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort),
                                              indices.Count, BufferUsage.None);

                indexBuffer.SetData(indices.ToArray());
                basicEffect = new BasicEffect(graphicsDevice);
            }

            #endregion

            #region Draw

            public virtual void Draw(Effect effect)
            {
                graphicsDevice = effect.GraphicsDevice;
                graphicsDevice.SetVertexBuffer(vertexBuffer);
                graphicsDevice.Indices = indexBuffer;

                foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
                {
                    effectPass.Apply();
                    int primitiveCount = indices.Count / 3;
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                         vertices.Count, 0, primitiveCount);
                }
            }

            public void Draw(Matrix world, Matrix view, Matrix projection, Texture2D texture)
            {
                // Set BasicEffect parameters.
                basicEffect.World = world;
                basicEffect.View = view;
                basicEffect.Projection = projection;
                basicEffect.TextureEnabled = true;
                basicEffect.Texture = texture;
                basicEffect.EnableDefaultLighting();
                //basicEffect.DiffuseColor = new Vector3(1, 0, 0);

                GraphicsDevice device = basicEffect.GraphicsDevice;
                device.DepthStencilState = DepthStencilState.Default;

                // Draw the model, using BasicEffect.
                Draw(basicEffect);
            }

            #endregion
        }
    }
