#define DRAW_SPIKES

using LifeFall.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LifeFall.Logic
{
    public class CModel
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Matrix RotationMatrix { get; set; }
        public Vector3 Scale { get; set; }


        public PhysicsModel PhysicsModel;
        public Model Model { get; set; }
        public bool textureEnabled = false;
        public Texture2D texture;


        private Matrix[] modelTransforms;
        private GraphicsDevice graphicsDevice;

        public CModel(Model Model, GraphicsDevice graphicsDevice)
        {
            InitCModel(Model, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), graphicsDevice);
        }

        public CModel(Model Model, Vector3 Position, Vector3 Rotation, 
            Vector3 Scale, GraphicsDevice graphicsDevice)
        {
            InitCModel(Model,  Position,  Rotation,  Scale, graphicsDevice);
            RotationMatrix = Matrix.Identity;
            //graphicsDevice.DepthStencilState = DepthStencilState.Default;
            
        }
        //BasicEffect bs;
        private void InitCModel(Model model, Vector3 position, Vector3 rotation,  Vector3 scale, GraphicsDevice graphicsDevice)
        {
            this.Model = model;
            modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.graphicsDevice = graphicsDevice;
            PhysicsModel = new PhysicsModel(model);
            //bs = new BasicEffect(graphicsDevice);
        }
        
        public void Draw(Matrix View, Matrix Projection)
        {
            Quaternion q = Quaternion.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
 
            Matrix baseWorld = Matrix.CreateScale(Scale)
            * RotationMatrix  *  Matrix.CreateFromQuaternion(q) 
            * Matrix.CreateTranslation(Position);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                * baseWorld;

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = localWorld;
                    effect.View = View;
                    effect.Projection = Projection;
                    

                    effect.TextureEnabled = false;
                    if (textureEnabled)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = texture;
                    }

                }
                mesh.Draw();
            }

#if DEBUG && DRAW_SPIKES
            if (ef == null)
                ef = new BasicEffect(graphicsDevice);
            Utils.DrawSphereSpikes((BoundingSphere)this.Model.Tag, graphicsDevice, ef, baseWorld, View, Projection);
#endif
            
        }
        BasicEffect ef;
        
        public void DrawOnPosition(Matrix View, Matrix Projection, Vector3 position)
        {
            //Vector3 tmpPosition = Position;

            //Position = position;

            Quaternion q = Quaternion.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Matrix baseWorld = Matrix.CreateScale(Scale)
            * RotationMatrix * Matrix.CreateFromQuaternion(q)
            * Matrix.CreateTranslation(position);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                * baseWorld;

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = localWorld;
                    effect.View = View;
                    effect.Projection = Projection;


                    effect.TextureEnabled = false;
                    if (textureEnabled)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = texture;
                    }

                }
                mesh.Draw();
            }

            //Position = tmpPosition;

        }

    }


}