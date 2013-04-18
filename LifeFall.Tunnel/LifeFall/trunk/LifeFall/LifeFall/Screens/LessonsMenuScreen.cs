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
#endregion

namespace LifeFall
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class LessonsMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public LessonsMenuScreen()
            : base("")
        {
            this.titleScale = 2f;
            // Create our menu entries.
            MenuEntry lesson1GameMenuEntry = new MenuEntry("Lesson 1: Movement in the vein.");
            MenuEntry lesson2GameMenuEntry = new MenuEntry("Lesson 2: Meeting red blood cells.");
            MenuEntry lesson3GameMenuEntry = new MenuEntry("Lesson 3: Fighting viruses.");
            MenuEntry exitMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            lesson1GameMenuEntry.Selected += Lesson1PlayGameMenuEntrySelected;
            lesson2GameMenuEntry.Selected += Lesson2PlayGameMenuEntrySelected;
            lesson3GameMenuEntry.Selected += Lesson3PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(lesson1GameMenuEntry);
            MenuEntries.Add(lesson2GameMenuEntry);
            MenuEntries.Add(lesson3GameMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void Lesson1PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LessonScreen(1,"Movement in the vein."), e.PlayerIndex);
        }

        void Lesson2PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LessonScreen(2,"Meeting red blood cells."), e.PlayerIndex);
        }

        void Lesson3PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LessonScreen(3,"Fighting viruses."), e.PlayerIndex);
        }


        #endregion
    }
}
