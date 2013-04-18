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
    class CreditsMenuScreen : MenuScreen
    {

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public CreditsMenuScreen()
            : base("")
        {
            // Create our menu entries.
            this.titleScale = 2f;
            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.

            back.Selected += OnCancel;

            MenuEntries.Add(back);
        }


        #endregion

        #region Handle Input


        #endregion

        #region Update and Draw


        public override void DrawTitleRectangle(SpriteBatch spriteBatch)
        {
            int width = (2 * ScreenManager.GraphicsDevice.Viewport.Width) / 3;
            int height = ScreenManager.GraphicsDevice.Viewport.Height / 5 - 2 * screenOffset;

            int x = (ScreenManager.GraphicsDevice.Viewport.Width / 3) / 2;
            int y = screenOffset;


            titleRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.title, titleRectangle, Color.White * TransitionAlpha);
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            Color lineColor = Color.Gold * TransitionAlpha;
            Color titleLineColor = Color.Red * TransitionAlpha;
            Color transitionAlphaColor = new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha);

            string rex = "Piotr Leniartek         Mateusz Nowak";
            //string mats = "Mateusz Nowak";
            string pucio = "Paulina Lecka         Paulina Misztal";
            //string paula = "Paulina Misztal";
            string artur = "Artur Staszczyk";

            string team = "Team";
            string mentor = "Mentor";
            string credits = "Thanks to";

            int x = viewport.Width / 2 - GetWidth(team)/2;
            int y = titleRectangle.Y + titleRectangle.Height;

            Vector2 position = new Vector2(x, y);

            spriteBatch.DrawString(font, team, position, titleLineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            x = viewport.Width / 2 - GetWidth(rex)/2;
            y = (int)position.Y +  GetHeight();

            position = new Vector2(x, y);
            spriteBatch.DrawString(font, rex, position, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            //x = viewport.Width / 2;
            //y = (int)position.Y + GetHeight();

            //position = new Vector2(x, y);
            //spriteBatch.DrawString(font, mats, position, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            x = viewport.Width / 2 - GetWidth(pucio)/2;
            y = (int)position.Y + GetHeight();

            position = new Vector2(x, y);
            spriteBatch.DrawString(font, pucio, position, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            //x = viewport.Width / 2;
            //y = (int)position.Y + GetHeight();

            //position = new Vector2(x, y);
            //spriteBatch.DrawString(font, paula, position, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            x = viewport.Width / 2 - GetWidth(mentor) / 2;
            y = (int)position.Y + GetHeight();

            position = new Vector2(x, y);
            spriteBatch.DrawString(font, mentor, position, titleLineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            x = viewport.Width / 2 - GetWidth(artur) / 2;
            y = (int)position.Y + GetHeight();

            position = new Vector2(x, y);
            spriteBatch.DrawString(font, artur, position, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            x = viewport.Width / 2 - GetWidth(credits) / 2;
            y = (int)position.Y + GetHeight();

            position = new Vector2(x, y);
            spriteBatch.DrawString(font, credits, position, titleLineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            string xnabuttons = "Jeff Jenkins for XNA Button Pack at sinnix.net";

            x = viewport.Width / 2 - GetWidth(xnabuttons) / 2;
            y = (int)position.Y + GetHeight();

            position = new Vector2(x, y);
            spriteBatch.DrawString(font, xnabuttons, position, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            string freesound = "freesound.org for nice sound effect's";

            x = viewport.Width / 2 - GetWidth(freesound) / 2;
            y = (int)position.Y + GetHeight();

            position = new Vector2(x, y);
            spriteBatch.DrawString(font, freesound, position, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            string music = "DST from nosoapradio.us for great music";

            x = viewport.Width / 2 - GetWidth(music) / 2;
            y = (int)position.Y + GetHeight();

            position = new Vector2(x, y);
            spriteBatch.DrawString(font, music, position, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);

            string turbosquid = "turbosquid.com for perfect models";

            x = viewport.Width / 2 - GetWidth(turbosquid) / 2;
            y = (int)position.Y + GetHeight();

            position = new Vector2(x, y);
            spriteBatch.DrawString(font, turbosquid, position, lineColor, 0, vector00, 1f, SpriteEffects.None, 0);



            // Draw each menu entry in turn.

            UpdateMenuEntryLocations((int)position.Y + 2 * GetHeight());

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }
        }

        #endregion
    }
}


