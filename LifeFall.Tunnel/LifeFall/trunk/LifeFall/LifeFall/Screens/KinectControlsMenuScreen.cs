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
#endregion

namespace LifeFall
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    class KinectControlsMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public KinectControlsMenuScreen()
            : base("")
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            // Create our menu entries.

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.

            back.Selected += OnCancel;

            MenuEntries.Add(back);

        }


        #endregion

        #region Handle Input


        #endregion

        #region Update and Draw

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            


        }

        private void drawUpDownRectangle(SpriteBatch spriteBatch, string up, string down, Rectangle rectangle)
        {
            SpriteFont font = ScreenManager.Font;

            int x = rectangle.X + rectangle.Width / 2 - GetWidth(up) / 2;
            int y = rectangle.Y - GetHeight();

            Vector2 textPosition = new Vector2(x, y);

            spriteBatch.DrawString(font, up, textPosition, Color.Gold, 0, vector00, 1f, SpriteEffects.None, 0);

            x = rectangle.X + rectangle.Width / 2 - GetWidth(down) / 2;
            y = rectangle.Y + rectangle.Height;

            textPosition = new Vector2(x, y);

            spriteBatch.DrawString(font, down, textPosition, Color.Gold, 0, vector00, 1f, SpriteEffects.None, 0);
        }


        #endregion
    }




}



