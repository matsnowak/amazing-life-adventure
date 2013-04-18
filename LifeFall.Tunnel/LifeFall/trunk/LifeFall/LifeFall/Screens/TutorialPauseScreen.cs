#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LifeFall.Logic;
using LifeFall.Core;
#endregion

namespace LifeFall
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class TutorialPauseScreen : GameScreen
    {
        #region Initialization

        ContentManager content;

        Texture2D backgroundTexture;
        MovableObject movableObject;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TutorialPauseScreen(MovableObject _movableObject)
        {
            movableObject = _movableObject;

            //// Create our menu entries.
            //MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            //MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            //// Hook up menu event handlers.
            //resumeGameMenuEntry.Selected += OnCancel;
            //quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            //// Add entries to the menu.
            //MenuEntries.Add(resumeGameMenuEntry);
            //MenuEntries.Add(quitGameMenuEntry);
        }


        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>("interfejsInvisible");
        }



        #endregion


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            int x = viewport.Width / 2 - backgroundTexture.Width / 2;
            int y = viewport.Height / 2 - backgroundTexture.Height / 2;

            Rectangle centeredScreen = new Rectangle(x, y, backgroundTexture.Width, backgroundTexture.Height);





            //2D Draw

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, centeredScreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            string type = movableObject.GetType().ToString().Replace("LifeFall.Logic.Blood.", "");


            //spriteBatch.DrawString(ScreenManager.Font, type, new Vector2(viewport.Width / 2, viewport.Height / 2), Color.White);

            Vector2 position = new Vector2(viewport.Width / 2 + 25, viewport.Height / 2 - backgroundTexture.Height / 2 + 12);

            spriteBatch.DrawString(ScreenManager.Font, type, position, Color.White, 0, vector00, 0.5f, SpriteEffects.None, 0);

            spriteBatch.End();




            //3D Draw
            ICamera cam = Costam.CameraManager.GetActiveCamera();

            Player player = null;
            foreach (Player P in Costam.PlayerManager.players.Values)
            {
                player = P;
                break;
            }


            Vector3 position3 = new Vector3(player.Position.X, player.Position.Y, player.Position.Z - 50);


            movableObject.CModel.DrawOnPosition(cam.ViewMatrix,cam.ProjectionMatrix, position3);


        }


        #region Handle Input

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            if (input.IsMenuCancel(ControllingPlayer, out playerIndex) || input.IsMenuSelect(ControllingPlayer,out playerIndex))
            {
                OnCancel(playerIndex);
            }
        }

        void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }

        #endregion
    }
}
