using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LifeFall.Models;

namespace LifeFall
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 900;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "LifeFall...";

            camera = new Camera(this, new Vector3(0, 0, 50), new Vector3(0, 0, 0), Vector3.Up);
            Components.Add(camera);

            tunnelPath = new TunnelPath();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            effect = new BasicEffect(GraphicsDevice);

            vectors = new VertexPositionColor[2];
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 2, BufferUsage.None);
            vertexBuffer.SetData(vectors);

            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            tunnelPath.AddSegment();
            //tunnelPath.DeleteSegment();
            //tunnelPath.DeleteSegment(1000000000);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code he

            //GraphicsDevice.SetVertexBuffer(vertexBuffer);

            effect.World = Matrix.Identity;
            effect.View = camera.view;
            effect.Projection = camera.projection;
            effect.VertexColorEnabled = true;


            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                tunnelPath.DrawTunnelPath(GraphicsDevice);
            }

            base.Draw(gameTime);
        }

        public Camera camera;
        public TunnelPath tunnelPath;


        VertexBuffer vertexBuffer;
        VertexPositionColor[] vectors;

        BasicEffect effect;
    }
}
