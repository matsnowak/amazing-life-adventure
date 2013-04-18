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
    class ControlsMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public ControlsMenuScreen()
            :base("")
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


            //objects rectangle
            int x = titleRectangle.X;
            int y = titleRectangle.Y + titleRectangle.Height + screenOffset;
            int width = titleRectangle.Width;
            int height = (3 * ((4 * viewport.Height) / 5)) / 7 - screenOffset;

            Rectangle objectsRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.redFolder, objectsRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));


            //controls rectangle's

            //keyboard rectangle
            x = titleRectangle.X;
            y = objectsRectangle.Y + objectsRectangle.Height + screenOffset;
            width = titleRectangle.Width / 2 - screenOffset / 2;
            height = objectsRectangle.Height;

            Rectangle keyboardRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.redFolder, keyboardRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            //controller rectangle
            x = keyboardRectangle.X + keyboardRectangle.Width + screenOffset;
            y = keyboardRectangle.Y;
            width = keyboardRectangle.Width;
            height = keyboardRectangle.Height;

            Rectangle controlerRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.redFolder, controlerRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));


            //OBJECTS
            int objectsOffset = 30;
            width = (objectsRectangle.Width - 6 * objectsOffset) / 5;
            height = objectsRectangle.Height - (2 * objectsOffset) - (2 * GetHeight());


            //bloodcell

            x = objectsRectangle.X + objectsOffset;
            y = objectsRectangle.Y + objectsRectangle.Height - objectsOffset - height - GetHeight();

            Rectangle bloodCellRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.bloodCell2D, bloodCellRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            drawUpDownRectangle(spriteBatch, "AVOID", "Blood Cell", bloodCellRectangle);

            //virus

            x = bloodCellRectangle.X + bloodCellRectangle.Width + objectsOffset;
            y = bloodCellRectangle.Y;

            Rectangle virusRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.virus2D, virusRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            drawUpDownRectangle(spriteBatch, "DESTROY", "Virus", virusRectangle);

            //gold diamond

            x = virusRectangle.X + virusRectangle.Width + objectsOffset;
            y = virusRectangle.Y;

            Rectangle goldDiamondRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.goldDiamond2D, goldDiamondRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            drawUpDownRectangle(spriteBatch, "", "Gold", goldDiamondRectangle);

            //blue diamond

            x = goldDiamondRectangle.X + goldDiamondRectangle.Width + objectsOffset;
            y = goldDiamondRectangle.Y;

            Rectangle blueDiamondRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.blueDiamond2D, blueDiamondRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            drawUpDownRectangle(spriteBatch, "COLLECT DIAMONDS", "Red", blueDiamondRectangle);

            //green diamond

            x = blueDiamondRectangle.X + blueDiamondRectangle.Width + objectsOffset;
            y = blueDiamondRectangle.Y;

            Rectangle greenDiamondRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.greemDiamond2D, greenDiamondRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            drawUpDownRectangle(spriteBatch, "", "Green", greenDiamondRectangle);


            //controls
            //keyboard

            string keyboard = "Keyboard";
            x = keyboardRectangle.X + keyboardRectangle.Width / 2 - GetWidth(keyboard) / 2;
            y = keyboardRectangle.Y;

            spriteBatch.DrawString(font, keyboard, new Vector2(x, y), Color.Gold * TransitionAlpha, 0, vector00, 1f, SpriteEffects.None, 0);

            x = keyboardRectangle.X + objectsOffset;
            y = keyboardRectangle.Y + keyboardRectangle.Height / 4;

            width = keyboardRectangle.Width / 2 - 2 * objectsOffset;
            height = keyboardRectangle.Height / 2;

            Rectangle arrowsRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.ARROWS, arrowsRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            drawUpDownRectangle(spriteBatch, "", "Move", arrowsRectangle);

            width = arrowsRectangle.Width / 2;
            height = arrowsRectangle.Height / 2;

            x = keyboardRectangle.X + keyboardRectangle.Width - objectsOffset - width;
            y = arrowsRectangle.Y + arrowsRectangle.Height / 4;

            Rectangle attackRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.SPACE_ALT, attackRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            string attack = "Attack";

            drawUpDownRectangle(spriteBatch, "", attack, attackRectangle);

            //controller
            string controller = "Controller";
            x = controlerRectangle.X + controlerRectangle.Width / 2 - GetWidth(controller) / 2;
            y = controlerRectangle.Y;

            spriteBatch.DrawString(font, controller, new Vector2(x, y), Color.Gold * TransitionAlpha, 0, vector00, 1f, SpriteEffects.None, 0);

            x = controlerRectangle.X + objectsOffset;
            y = controlerRectangle.Y + controlerRectangle.Height / 4;

            width = controlerRectangle.Width / 2 - 2 * objectsOffset;
            height = controlerRectangle.Height / 2;

            Rectangle dpadRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.DPAD_LR, dpadRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            drawUpDownRectangle(spriteBatch, "", "Move", dpadRectangle);

            width = arrowsRectangle.Width / 2;
            height = arrowsRectangle.Height / 2;

            x = controlerRectangle.X + controlerRectangle.Width - objectsOffset - width;
            y = dpadRectangle.Y + dpadRectangle.Height / 4;

            Rectangle aAttackRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.A, aAttackRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            drawUpDownRectangle(spriteBatch, "", attack, aAttackRectangle);

            

            //x = jumpRectangle.X;
            //y = jumpRectangle.Y + jumpRectangle.Height;

            //width = jumpRectangle.Width;
            //height = jumpRectangle.Height;

            //Rectangle attackRectangle = new Rectangle(x, y, width, height);

            //spriteBatch.Draw(Costam.B, attackRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            //x = attackRectangle.X + attackRectangle.Width;
            //y = attackRectangle.Y + attackRectangle.Height / 2 - GetHeight() / 2;

            //spriteBatch.DrawString(font, attack, new Vector2(x, y), Color.Gold * TransitionAlpha, 0, vector00, 1f, SpriteEffects.None, 0);

            // Draw each menu entry in turn.

            UpdateMenuEntryLocations(controlerRectangle.Y + controlerRectangle.Height + screenOffset);

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }
        }

        private void drawUpDownRectangle(SpriteBatch spriteBatch, string up, string down, Rectangle rectangle)
        {
            SpriteFont font = ScreenManager.Font;

            int x = rectangle.X + rectangle.Width / 2 - GetWidth(up) / 2;
            int y = rectangle.Y - GetHeight();

            Vector2 textPosition = new Vector2(x, y);

            spriteBatch.DrawString(font, up, textPosition, Color.Gold * TransitionAlpha, 0, vector00, 1f, SpriteEffects.None, 0);

            x = rectangle.X + rectangle.Width / 2 - GetWidth(down) / 2;
            y = rectangle.Y + rectangle.Height;

            textPosition = new Vector2(x, y);

            spriteBatch.DrawString(font, down, textPosition, Color.Gold * TransitionAlpha, 0, vector00, 1f, SpriteEffects.None, 0);
        }


        #endregion

    }




}


