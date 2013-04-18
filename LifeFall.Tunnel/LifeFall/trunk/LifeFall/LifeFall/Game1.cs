//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using LifeFall.Core;
//using LifeFall.Core.Trigger_System;
//using Jitter.Collision;
//using Jitter.Dynamics;
//using Jitter.LinearMath;
//using Jitter.Collision.Shapes;
//using Jitter;
//using LifeFall.Core.Controls;
//using LifeFall.Debug;
//using LifeFall.Logic;
//using LifeFall.Logic.Blood;

//namespace LifeFall
//{
//    /// <summary>
//    /// This is the main type for your game
//    /// </summary>
//    public class Game1 : Microsoft.Xna.Framework.Game
//    {
//        GraphicsDeviceManager graphics;
//        public SpriteBatch spriteBatch;

//        Texture2D otherTexture;

//        public KeyboardState keyboardState;

//        public CameraManager cameraManager;
//        public TunnelManager tunnelManager;
//        public TriggerManager triggerManager;
//        public ObjectManager objectManager;
//        public PlayerManager playerManager;
//        public CollisionManager collisionManager;

//        public SimpleMemoryPool objectsProvider;

//        public Hud hud;
//        public DebugConsole DebugTextWindow;
//        public static string DebugDrawWindowName = "DebugConsole";

//        public DebugDraw debugDraw;


//        #region Game Configuration

//        const int tunnelPathTessellation = 20;
//        const int tunnelPathRadius = 40;
//        const int tunnelPathInterval = 60;
//        const int tunnelPathSegmentsToAddInOnePart = 3;

//        const int tunnelTessellation = 32;
//        const int numberOfTunnelVertices = (tunnelPathSegmentsToAddInOnePart * 4) * tunnelPathTessellation * tunnelTessellation;

//        #endregion

//        public Game1()
//        {
//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";

//            graphics.IsFullScreen = false;
//            Costam.Game = this;

//        }

//        /// <summary>
//        /// Allows the game to perform any initialization it needs to before starting to run.
//        /// This is where it can query for any required services and load any non-graphic
//        /// related content.  Calling base.Initialize will enumerate through any components
//        /// and initialize them as well.
//        /// </summary>
//        protected override void Initialize()
//        {
//            base.Initialize();

//        }

//        /// <summary>
//        /// LoadContent will be called once per game and is the place to load
//        /// all of your content.
//        /// </summary>
//        protected override void LoadContent()
//        {
//            #region Locals
//            spriteBatch = new SpriteBatch(GraphicsDevice);
//            otherTexture = Content.Load<Texture2D>("vein256");
//            #endregion Locals

//            #region Camera Manager

//            cameraManager = new CameraManager(this);
//            Costam.CameraManager = cameraManager;

//            DebugCamera camera = new Core.DebugCamera(this, new Vector3(-15, 0, 0),
//               Vector3.Zero, Vector3.Up);

//            ChaseCamera chaseCamera = new ChaseCamera(this);
            
//            cameraManager.AddCamera("DebugCamera", camera);
//            cameraManager.AddCamera("ChaseCamera", chaseCamera);

//            #endregion CameraManager

//            #region Triggers
//            triggerManager = new TriggerManager(this);
//            Costam.TriggerManager = triggerManager;

            


//            #endregion Triggers

//            #region Tunnel Manager
//            tunnelManager = new TunnelManager(this);
//            Costam.TunnelManager = tunnelManager;
//            #endregion TunnelManager

//            #region Memory Managment
//            objectsProvider = new SimpleMemoryPool();
//            Costam.MemoryPoolObjectProvider = objectsProvider;
//            #endregion Memory Managment

//            #region Player

//            playerManager = new PlayerManager(this);
//            Costam.PlayerManager = playerManager;

//            chaseCamera.ChasedObject = playerManager.players.ElementAt(0).Value;

//            Components.Add(playerManager);


//            #endregion PLayer

//            #region HUD


            //hud = new Hud(this, spriteBatch);
            //Costam.HUD = hud;


//            SpriteFont consoleFont = Content.Load<SpriteFont>("HudFont");
//            DebugTextWindow = new DebugConsole(new Rectangle(10, 10, 100, 50), consoleFont);
//            hud.AddComponent(DebugDrawWindowName, DebugTextWindow);
            
//            Vector2 windowDimensions = new Vector2(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

//            FPSCounter fpsCounter = new FPSCounter(new Rectangle((int) windowDimensions.X - 60, 5, 30, 20), consoleFont);
//            hud.AddComponent("FPS", fpsCounter);

//            if (playerManager.players.Count < 2)
//            {
//                PlayerHUD player1HUD = new PlayerHUD(new Rectangle((int)windowDimensions.X / 2, 0, 30, 20), consoleFont, playerManager.players.ElementAt(0).Value);
//                hud.AddComponent("player1HUD", player1HUD);
//            }
//            else
//            {
//                PlayerHUD player1HUD = new PlayerHUD(new Rectangle(((int)windowDimensions.X / 2 ) - 70, 0, 30, 20), consoleFont, playerManager.players.ElementAt(0).Value);
//                hud.AddComponent("player1HUD", player1HUD);


//                PlayerHUD player2HUD = new PlayerHUD(new Rectangle((int)windowDimensions.X / 2, 0, 30, 20), consoleFont, playerManager.players.ElementAt(1).Value);
//                hud.AddComponent("player2HUD", player2HUD);


//            }




//            #endregion HUD

//            #region Object Manager
//            objectManager = new ObjectManager(this);
//            Costam.ObjectManager = objectManager;
//            #endregion Object Manager

//            #region Colision Manager
//            collisionManager = new CollisionManager(this);
//            Costam.ColisionManager = collisionManager;
//            #endregion Colision Manager

//            #region Debug
//            debugDraw = new DebugDraw();
//            Costam.DebugDraw = debugDraw;
//            #endregion Debug
//        }

        
        

//        /// <summary>
//        /// UnloadContent will be called once per game and is the place to unload
//        /// all content.
//        /// </summary>
//        protected override void UnloadContent()
//        {
//        }


        //T funkcja<T>() where T: new()
        //{
        //    return new T();
        //}
        ///// <summary>
        ///// Allows the game to run logic such as updating the world,
        ///// checking for collisions, gathering input, and playing audio.
        ///// </summary>
        ///// <param name="gameTime">Provides a snapshot of timing values.</param>
        //protected override void Update(GameTime gameTime)
        //{
        //    // Allows the game to exit
        //    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        //        this.Exit();


//            keyboardState = Keyboard.GetState();

//            if (keyboardState.IsKeyDown(Keys.D1))
//            {
//                cameraManager.GetCamera("DebugCamera").Position = cameraManager.GetActiveCamera().Position;
//                cameraManager.SetActiveCamera("DebugCamera");

//            }


            


//            #region Player

            

//            //player.PhysicsModel.RigidBody.Position = new JVector(player.Position.X, player.Position.Y, player.Position.Z);
//            //obstacle.RigidBody.Position = new JVector(obstacle.Position.X, obstacle.Position.Y, obstacle.Position.Z);

            

//            #endregion Player


//            if (keyboardState.IsKeyDown(Keys.D3))
//            {
//                cameraManager.SetActiveCamera("ChaseCamera");
//            }

//            var camera = cameraManager.GetActiveCamera();

//            if (keyboardState.IsKeyDown(Keys.L))
//            {
//                tunnelManager.drawTunnelPath = !tunnelManager.drawTunnelPath;
//            }


//            tunnelManager.Update(gameTime);
//            playerManager.Update(gameTime);
//            cameraManager.Update(gameTime);
//            triggerManager.Update(gameTime);
//            objectManager.Update(gameTime);
//            collisionManager.Update(gameTime);
           
//#if DEBUG
//            hud.Update(gameTime);

//#endif
//            //base.Update(gameTime);
//        }


//        /// <summary>
//        /// This is called when the game should draw itself.
//        /// </summary>
//        /// <param name="gameTime">Provides a snapshot of timing values.</param>
//        protected override void Draw(GameTime gameTime)
//        {
           
//            ResetGraphicsDevice();
//            ICamera camera = cameraManager.GetActiveCamera();


//            playerManager.Draw(gameTime);
//            objectManager.Draw(gameTime);
//            tunnelManager.Draw(Matrix.Identity, camera.ViewMatrix, camera.ProjectionMatrix, otherTexture);
//#if DEBUG
//            Costam.DebugWrite("VirusPoolSize: " + Costam.MemoryPoolObjectProvider.VirusPool.Count.ToString());
//            Costam.DebugWrite("CellPoolSize: " + Costam.MemoryPoolObjectProvider.RedBloodCellPool.Count.ToString());
//            Costam.DebugWrite("PointsPoolSize: " + Costam.MemoryPoolObjectProvider.CollectiblePointPool.Count.ToString());
//            Costam.DebugWrite("DisposeCounter: " + Costam.MemoryPoolObjectProvider.DisposeCounter.ToString());
//            Costam.DebugWrite("GetObjectCounter: " + Costam.MemoryPoolObjectProvider.GetObjectCounter.ToString());
//            Costam.DebugWrite("OBjects.Count: " + Costam.ObjectManager.Obstacles.Count);
//            Costam.DebugWrite("Difference: " + (Costam.MemoryPoolObjectProvider.GetObjectCounter - (Costam.MemoryPoolObjectProvider.DisposeCounter + Costam.ObjectManager.Obstacles.Count)).ToString());
//             hud.Draw(gameTime);
//#endif

//        }


//        public void ResetGraphicsDevice()
//        {
//            GraphicsDevice.Clear(Color.DarkCyan);
//            DepthStencilState depthState = new DepthStencilState();
//            depthState.DepthBufferEnable = true; 
//            depthState.DepthBufferWriteEnable = true;

//            GraphicsDevice.DepthStencilState = DepthStencilState.Default ;
//            GraphicsDevice.BlendState = BlendState.Opaque;

//            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

//            RasterizerState r = new RasterizerState()
//            {
//                FillMode = FillMode.Solid,
//                CullMode = CullMode.None,
//                ScissorTestEnable = true
//            };
//            GraphicsDevice.RasterizerState = r;
//        }

//    }
//}
