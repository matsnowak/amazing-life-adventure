#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
#endregion

namespace LifeFall
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("")
        {
            this.titleScale = 2f;
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("PLAY");
            MenuEntry controlsMenyEntry = new MenuEntry("CONTROLS");
            //MenuEntry kinectControlsMenyEntry = new MenuEntry("KINECT CONTROL");
            //MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry creditssMenuEntry = new MenuEntry("CREDITS");
            MenuEntry exitMenuEntry = new MenuEntry("QUIT");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            controlsMenyEntry.Selected += ControlsMenuEntrySelected;
            //kinectControlsMenyEntry.Selected += KinectControlsMenyEntrySelected;
            //optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            creditssMenuEntry.Selected += CreditsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(controlsMenyEntry);
            //MenuEntries.Add(kinectControlsMenyEntry);
            //MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(creditssMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SelectGameMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// Event handler for when the Controls menu entry is selected.
        /// </summary>
        void ControlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlsMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Controls menu entry is selected.
        /// </summary>
        void KinectControlsMenyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new KinectControlsMenuScreen(), e.PlayerIndex);
        } 

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CreditsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            //const string message = "Are you sure you want to exit this sample?";

            //MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            //confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            //ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
            ScreenManager.Game.Exit();
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
