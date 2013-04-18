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
using System.Linq;
#endregion

namespace LifeFall
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class SelectGameMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public SelectGameMenuScreen()
            : base("")
        {
            this.titleScale = 2f;
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play & Learn !");
            MenuEntry endlesMenyEntry = new MenuEntry("Endlessssss Mode");
            MenuEntry exitMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            endlesMenyEntry.Selected += EndlesMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(endlesMenyEntry);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LessonsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// Event handler for when the Controls menu entry is selected.
        /// </summary>
        void EndlesMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            GameplayScreen g = new GameplayScreen(0);
            g.AfterLoadContent = delegate()
            {
                g.objectManager.InitBloodSequences();
                g.objectManager.InitDiamondSequences();
                g.objectManager.InitVirusesSequences();
                g.objectManager.VirusGenerator.Enabled = true;
                g.objectManager.BloodStreamGenerator.Enabled = true;
                g.playerManager.players.Values.ElementAt(0).ShootingEnabled = true;
            };
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, g);
        }


        #endregion
    }
}
