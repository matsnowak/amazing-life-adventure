using LifeFall.Core.Trigger_System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Core
{
    public class TunnelManager
    {
        Vector3[] checkPoints;
        PositionXReachedTrigger[] tunnelPositionTriggers;

        public Tunnel tunnel;
        public TunnelPath tp;

        public bool drawTunnelPath = true;

        GraphicsDevice graphicsDevice;
        Game game;

        const int tunnelPathTessellation = Costam.TUNNEL_PATH_TESSELLATION;
        const int tunnelPathRadius = Costam.TUNNEL_PATH_RADIUS;
        const int tunnelPathInterval = Costam.TUNNEL_PATH_INTERVAL;
        const int tunnelPathSegmentsToAddInOnePart = Costam.TUNNEL_PATH_ONE_PART_SEGMENTS;

        const int tunnelTessellation = Costam.TUNNEL_TESSELLATION;
        const int tunnelRadius = Costam.TUNNEL_RADIUS;
        const int numberOfTunnelVertices = (tunnelPathSegmentsToAddInOnePart * 4) * tunnelPathTessellation * tunnelTessellation;
        BasicEffect effect;

        public TunnelManager(Game game)
        {

            this.game = game;
            this.graphicsDevice = game.GraphicsDevice;
            tp = new Core.TunnelPath(tessellation: tunnelPathTessellation, interval: tunnelPathInterval, radius: tunnelPathRadius, length: tunnelPathSegmentsToAddInOnePart);
            tp.AddSegment(3 * tunnelPathSegmentsToAddInOnePart);


            checkPoints = new Vector3[3];
            tunnel = new Core.Tunnel(game.GraphicsDevice, tunnelTessellation, tp.PathPoints, tunnelRadius);
            InitTunnelPositionTriggers();
            UpdateCheckPoints();
            effect = new BasicEffect(graphicsDevice);

        }

        private void InitTunnelPositionTriggers()
        {
            //TODO: zmienic na playera
            ICamera cam = Costam.CameraManager.GetCamera("ChaseCamera");
            tunnelPositionTriggers = new PositionXReachedTrigger[3];

            for (int i = 0; i < tunnelPositionTriggers.Length; i++)
            {
                tunnelPositionTriggers[i] = new PositionXReachedTrigger(cam, 0f);
                tunnelPositionTriggers[i].Enabled = true;
                tunnelPositionTriggers[i].TriggerOnce = true;

            }
            tunnelPositionTriggers[0].Action = delegate()
            {
                AddNewPathPoints();

                tunnel.RemoveVerticesPart(0);
                tunnel.RemoveVerticesPart(0);
                tunnel.InitVerticesPart(2);
                
                

            };
            tunnelPositionTriggers[1].Action = delegate()
            {
                AddNewPathPoints();
                tunnel.InitVerticesPart(2);
                tunnel.StartDrawingNextPart();
                UpdateCheckPoints();
            };

            Costam.TriggerManager.AddTrigger(tunnelPositionTriggers[0]);
            Costam.TriggerManager.AddTrigger(tunnelPositionTriggers[1]);
            //Costam.TriggerManager.AddTrigger(tunnelPositionTriggers[2]);
        }


        void UpdateCheckPoints()
        {
            var tunellPathLenght = tp.PathPoints.Count;
            //for (int i = 0; i < checkPoints.Length; i++)
            //{
            //    checkPoints[i] = tp.PathPoints[(int)((i + 1) * tunellPathLenght / 4)];
            //    tunnelPositionTriggers[i].Position = checkPoints[i];
            //    tunnelPositionTriggers[i].Enabled = true;
            //}

            checkPoints[0] = tp.PathPoints[3 * tunellPathLenght / 8];
            tunnelPositionTriggers[0].Position = checkPoints[0];
            tunnelPositionTriggers[0].Enabled = true;

            checkPoints[1] = tp.PathPoints[5 * tunellPathLenght / 8];
            tunnelPositionTriggers[1].Position = checkPoints[1];
            tunnelPositionTriggers[1].Enabled = true;
        }

        public void Draw(Matrix world, Matrix view, Matrix projection, Texture2D texture)
        {
            tunnel.Draw(world, view, projection, texture);
#if DEBUG

#if DRAW_NORMALS
            tunnel.DrawNormals();
#endif 
            if (drawTunnelPath)
            {
                effect.World = Matrix.Identity;
                effect.View = view;
                effect.Projection = projection;
                effect.VertexColorEnabled = true;

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    tp.DrawTunnelPath(graphicsDevice);
                }
            }
#endif
        }

        public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// usuwa z tyłu i dodaje z przodu 1/4 punktów
        /// </summary>
        private void AddNewPathPoints()
        {

            tp.DeleteSegment(tunnelPathSegmentsToAddInOnePart);
            tp.AddSegment(tunnelPathSegmentsToAddInOnePart);
        }


    }
}
