#region File Description
//-----------------------------------------------------------------------------
// MenuScreen.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
#endregion

namespace LifeFall
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        List<MenuEntry> menuEntries = new List<MenuEntry>();
        public int selectedEntry = 0;
        string menuTitle;

        public float titleScale = 3f;

        public Rectangle titleRectangle;
        public int screenOffset;

        #endregion

        #region Properties


        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);







        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            // Move to the previous menu entry?
            if (input.IsMenuUp(ControllingPlayer))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;

                Costam.clickEffect.Play();
            }

            // Move to the next menu entry?
            if (input.IsMenuDown(ControllingPlayer))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;

                Costam.clickEffect.Play();
            }

            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                Costam.clickEffect.Play();
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                OnCancel(playerIndex);
            }
        }


        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 position = new Vector2(0f, viewport.Height / 2 - 50);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this) + menuEntry.blankSpace;
            }
        }

        protected virtual void UpdateMenuEntryLocations(int height)
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 position = new Vector2(0f, height);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this) + menuEntry.blankSpace;
            }
        }


        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //title
            screenOffset = 30;

            int width = (2 * ScreenManager.GraphicsDevice.Viewport.Width) / 3;
            int height = ScreenManager.GraphicsDevice.Viewport.Height / 5;

            int x = (ScreenManager.GraphicsDevice.Viewport.Width / 3) / 2;
            int y = screenOffset;


            titleRectangle = new Rectangle(x, y, width, height);



            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            DrawTitleRectangle(spriteBatch);

            DrawContent(gameTime, spriteBatch);

            DrawDownButtons(spriteBatch);

            spriteBatch.End();
        }

        public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch)
        {
            UpdateMenuEntryLocations();

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }
        }

        public override void DrawTitleRectangle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Costam.title, titleRectangle, Color.White * TransitionAlpha);
        }

        public override void DrawDownButtons(SpriteBatch spriteBatch)
        {
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            Color lineColor = Color.Gold * TransitionAlpha;
            Color titleLineColor = Color.Red * TransitionAlpha;
            Color transitionAlphaColor = new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha);

            MenuEntry lastMenuEntry = MenuEntries[MenuEntries.Count - 1];

            int y = (int)lastMenuEntry.Position.Y - GetHeight() / 2;
            int x = titleRectangle.X;

            Rectangle aRectangle = new Rectangle(x, y, 2 * GetHeight(), 2 * GetHeight());
            spriteBatch.Draw(Costam.A, aRectangle, transitionAlphaColor);

            x = aRectangle.X + aRectangle.Width;
            y = aRectangle.Y + aRectangle.Height / 2 - GetHeight() / 2;

            Vector2 playPos = new Vector2(x, y);
            spriteBatch.DrawString(font, "Select", playPos, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            y = (int)lastMenuEntry.Position.Y;
            x = titleRectangle.X + titleRectangle.Width - GetWidth("Back");

            Vector2 backPos = new Vector2(x, y);
            spriteBatch.DrawString(font, "Back", backPos, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            x = (int)backPos.X - aRectangle.Width;
            y = aRectangle.Y;

            Rectangle bRectangle = new Rectangle(x, y, aRectangle.Width, aRectangle.Height);
            spriteBatch.Draw(Costam.B, bRectangle, transitionAlphaColor);
        }

        #endregion
    }
}
