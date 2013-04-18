#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
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
using LifeFall.Core;
using LifeFall.Core.Trigger_System;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;
using Jitter.Collision.Shapes;
using Jitter;
using LifeFall.Core.Controls;
using LifeFall.Debug;
using LifeFall.Logic;
using LifeFall.Logic.Blood;
#endregion

namespace LifeFall
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        public SpriteBatch spriteBatch;

        public Texture2D otherTexture;

        public KeyboardState keyboardState;

        public CameraManager cameraManager;
        public TunnelManager tunnelManager;
        public TriggerManager triggerManager;
        public ObjectManager objectManager;
        public PlayerManager playerManager;
        public CollisionManager collisionManager;

        public SimpleMemoryPool objectsProvider;

        public Hud hud;
        public DebugConsole DebugTextWindow;
        public static string DebugDrawWindowName = "DebugConsole";

        public DebugDraw debugDraw;



        #region Game Configuration

        const int tunnelPathTessellation = 20;
        const int tunnelPathRadius = 40;
        const int tunnelPathInterval = 60;
        const int tunnelPathSegmentsToAddInOnePart = 3;

        const int tunnelTessellation = 32;
        const int numberOfTunnelVertices = (tunnelPathSegmentsToAddInOnePart * 4) * tunnelPathTessellation * tunnelTessellation;

        #endregion

        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        float pauseAlpha;

        bool gameOver = false;

        int lesson;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(int lesson)
        {
            this.lesson = lesson;
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            MediaPlayer.Play(Costam.gameSong);

        }

        public delegate void afterLoadContent();
        public afterLoadContent AfterLoadContent;
        
        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.

            #region Locals
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            otherTexture = ScreenManager.Game.Content.Load<Texture2D>("vein256");
            #endregion Locals

            #region Camera Manager

            cameraManager = new CameraManager(ScreenManager.Game);
            Costam.CameraManager = cameraManager;

            DebugCamera camera = new Core.DebugCamera(ScreenManager.Game, new Vector3(-15, 0, 0),
               Vector3.Zero, Vector3.Up);

            ChaseCamera chaseCamera = new ChaseCamera(ScreenManager.Game);

            cameraManager.AddCamera("DebugCamera", camera);
            cameraManager.AddCamera("ChaseCamera", chaseCamera);

            #endregion CameraManager

            #region Triggers
            triggerManager = new TriggerManager(ScreenManager.Game);
            Costam.TriggerManager = triggerManager;

            #endregion Triggers

            #region Tunnel Manager
            tunnelManager = new TunnelManager(ScreenManager.Game);
            Costam.TunnelManager = tunnelManager;
            #endregion TunnelManager

            #region Memory Managment
            objectsProvider = new SimpleMemoryPool();
            Costam.MemoryPoolObjectProvider = objectsProvider;
            #endregion Memory Managment

            #region Player

            playerManager = new PlayerManager(ScreenManager.Game);
            Costam.PlayerManager = playerManager;

            chaseCamera.ChasedObject = Costam.PlayerManager.players.ElementAt(0).Value;


            #endregion PLayer

            #region HUD

            hud = new Hud(ScreenManager.Game, spriteBatch);
            Costam.HUD = hud;
            //ScreenManager.Game.Components.Add(hud);

            SpriteFont consoleFont = ScreenManager.Game.Content.Load<SpriteFont>("HudFont");
            DebugTextWindow = new DebugConsole(new Rectangle(10, 10, 100, 50), consoleFont);
            hud.AddComponent(DebugDrawWindowName, DebugTextWindow);

            Vector2 windowDimensions = new Vector2(ScreenManager.Game.GraphicsDevice.Viewport.Width, ScreenManager.Game.GraphicsDevice.Viewport.Height);

            FPSCounter fpsCounter = new FPSCounter(new Rectangle((int)windowDimensions.X - 60, 5, 30, 20), consoleFont);
            hud.AddComponent("FPS", fpsCounter);

            if (playerManager.players.Count < 2)
            {
                PlayerHUD player1HUD = new PlayerHUD(new Rectangle((int)windowDimensions.X / 2, 0, 30, 20), consoleFont, playerManager.players.ElementAt(0).Value);
                hud.AddComponent("player1HUD", player1HUD);
            }
            else
            {
                PlayerHUD player1HUD = new PlayerHUD(new Rectangle(((int)windowDimensions.X / 2) - 70, 0, 30, 20), consoleFont, playerManager.players.ElementAt(0).Value);
                hud.AddComponent("player1HUD", player1HUD);


                PlayerHUD player2HUD = new PlayerHUD(new Rectangle((int)windowDimensions.X / 2, 0, 30, 20), consoleFont, playerManager.players.ElementAt(1).Value);
                hud.AddComponent("player2HUD", player2HUD);


            }




            #endregion HUD

            #region Object Manager
            objectManager = new ObjectManager(Costam.Game);
            Costam.ObjectManager = objectManager;
            #endregion Object Manager

            #region Colision Manager
            collisionManager = new CollisionManager(Costam.Game);
            Costam.ColisionManager = collisionManager;
            #endregion Colision Manager

            #region Debug
            debugDraw = new DebugDraw();
            Costam.DebugDraw = debugDraw;

            Costam.Random = new Random((int) System.DateTime.Now.Ticks);
            #endregion Debug

            AfterLoadContent();


            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                Costam.TunnelManager.Update(gameTime);
                Costam.PlayerManager.Update(gameTime);
                Costam.CameraManager.Update(gameTime);
                Costam.TriggerManager.Update(gameTime);
                Costam.ObjectManager.Update(gameTime);
                Costam.ColisionManager.Update(gameTime);
#if DEBUG
                Costam.HUD.Update(gameTime);
#endif

                Player player = Costam.PlayerManager.players.ElementAt(0).Value;
#if !DEBUG
                if (player.health == 0)
                {
                    gameOver = true;

                    ScreenManager.AddScreen(new GameOverScreen(player.score, "Game Over", true, lesson), ControllingPlayer);
                }
#endif


            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (!gameOver)
            {

            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];



            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            //ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
            //                                   Color.CornflowerBlue, 0, 0);


            ResetGraphicsDevice();
            ICamera camera = Costam.CameraManager.GetActiveCamera();

            Costam.PlayerManager.Draw(gameTime);
            Costam.ObjectManager.Draw(gameTime);
            Costam.TunnelManager.Draw(Matrix.Identity, camera.ViewMatrix, camera.ProjectionMatrix, otherTexture);





            #region UI
            spriteBatch.Begin();



            Player player = Costam.PlayerManager.players.ElementAt(0).Value;
            int health = (int)player.health;

            float height = ScreenManager.GraphicsDevice.Viewport.Height * 0.08f;
            int intHeight = (int)height;

            if (health <= 0)
            {
                spriteBatch.Draw(Costam.noLife, new Rectangle(0, 0, Costam.noLife.Width, Costam.noLife.Height), Color.White);
            }
            if (health == 1)
            {
                spriteBatch.Draw(Costam.oneLife, new Rectangle(0, 0, Costam.oneLife.Width, Costam.oneLife.Height), Color.White);
            }
            if (health == 2)
            {
                spriteBatch.Draw(Costam.twoLife, new Rectangle(0, 0, Costam.twoLife.Width, Costam.twoLife.Height), Color.White);
            }
            if (health == 3)
            {
                spriteBatch.Draw(Costam.threeLife, new Rectangle(0, 0, Costam.threeLife.Width, Costam.threeLife.Height), Color.White);
            }


            spriteBatch.Draw(Costam.scoreBar, new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width - Costam.scoreBar.Width, 0, Costam.scoreBar.Width, Costam.scoreBar.Height), Color.Gold);

            Vector2 scorePosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - GetWidth(player.score.ToString()) - 5 , Costam.scoreBar.Height/2 - GetHeight()/2);

            spriteBatch.DrawString(ScreenManager.Font, player.score.ToString(), scorePosition, Color.Gold);



            spriteBatch.End();
            #endregion UI




#if DEBUG
            Costam.HUD.Draw(gameTime);
            Costam.DebugWrite("VirusPoolSize: " + Costam.MemoryPoolObjectProvider.poolContainter[typeof(Virus)].Count.ToString());
            Costam.DebugWrite("CellPoolSize: " + Costam.MemoryPoolObjectProvider.poolContainter[typeof(RedBloodCell)].Count.ToString());
            Costam.DebugWrite("PointsPoolSize: " + Costam.MemoryPoolObjectProvider.poolContainter[typeof(CollectiblePoint)].Count.ToString());
            Costam.DebugWrite("DisposeCounter: " + Costam.MemoryPoolObjectProvider.DisposeCounter.ToString());
            Costam.DebugWrite("GetObjectCounter: " + Costam.MemoryPoolObjectProvider.GetObjectCounter.ToString());
            Costam.DebugWrite("OBjects.Count: " + Costam.ObjectManager.Obstacles.Count);
            Costam.DebugWrite("Difference: " + (Costam.MemoryPoolObjectProvider.GetObjectCounter - (Costam.MemoryPoolObjectProvider.DisposeCounter + Costam.ObjectManager.Obstacles.Count)).ToString());
#endif


            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        int GetHeight()
        {
            return ScreenManager.Font.LineSpacing;
        }


        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        int GetWidth(string text)
        {
            return (int)ScreenManager.Font.MeasureString(text).X;
        }

        public void ResetGraphicsDevice()
        {
            ScreenManager.GraphicsDevice.Clear(Color.Black);
            DepthStencilState depthState = new DepthStencilState();
            depthState.DepthBufferEnable = true;
            depthState.DepthBufferWriteEnable = true;

            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;

            ScreenManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            RasterizerState r = new RasterizerState()
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };
            ScreenManager.GraphicsDevice.RasterizerState = r;
        }


        #endregion
    }
}
