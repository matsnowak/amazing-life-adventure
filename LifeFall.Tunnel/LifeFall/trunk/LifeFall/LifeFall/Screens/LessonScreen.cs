#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using LifeFall.Core;
using LifeFall.Core.Trigger_System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
#endregion

namespace LifeFall
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class LessonScreen : MenuScreen
    {
        #region Initialization

        int lesson;
        string title;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public LessonScreen(int _lesson, string _title)
            : base("")
        {
            lesson = _lesson;
            title = _title;

            this.titleScale = 2f;
            // Create our menu entries.

            MenuEntry playGameMenuEntry = new MenuEntry("Play");
            MenuEntry exitMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);






        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            switch (lesson)
            {
                case 1:
                    {
                        GameplayScreen l1 = new GameplayScreen(1);
                        float minScore = 150;
                        l1.AfterLoadContent = delegate()
                        {
                            PlayerManager pm = Costam.PlayerManager;
                            Player pl = Costam.PlayerManager.players.Values.ElementAt(0);
                            pl.ShootingEnabled = false;
                            l1.objectManager.InitDiamondSequences();
                            l1.objectManager.VirusGenerator.Enabled = false;
                            Trigger lessonOneTrigger = new Trigger();
                            lessonOneTrigger.Condition = delegate()
                            {
                                return pl.score >= minScore;
                            };

                            lessonOneTrigger.Action = delegate()
                            {
                                lessonOneTrigger.Enabled = false;
                                
                                ScreenManager.AddScreen(new GameOverScreen(pl.score, "Excellent!",false,1), ControllingPlayer);
                            };

                            Costam.TriggerManager.AddTrigger(lessonOneTrigger);
                        };


                        LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, l1);
                    }
                    break;
                case 2:
                    {
                        GameplayScreen l2 = new GameplayScreen(2);
                        float minScore = 150;
                        l2.AfterLoadContent = delegate()
                        {
                            PlayerManager pm = Costam.PlayerManager;
                            Player pl = Costam.PlayerManager.players.Values.ElementAt(0);
                            pl.ShootingEnabled = false;

                            l2.objectManager.InitDiamondSequences();
                            l2.objectManager.InitBloodSequences();
                            l2.objectManager.VirusGenerator.Enabled = false;
                            Trigger lessonTwoTrigger = new Trigger();
                            lessonTwoTrigger.Condition = delegate()
                            {
                                return (pl.score >= minScore) && pl.CollisionWithRedBLoodCellHappened;
                            };

                            lessonTwoTrigger.Action = delegate()
                            {
                                lessonTwoTrigger.Enabled = false;

                                ScreenManager.AddScreen(new GameOverScreen(pl.score, "Congratulations!", false,2), ControllingPlayer);
                            };

                            Costam.TriggerManager.AddTrigger(lessonTwoTrigger);
                        };


                        LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, l2);

                    }
                    break;

                case 3:
                    {
                        GameplayScreen l3 = new GameplayScreen(3);
                        float killViruses = 10;
                        l3.AfterLoadContent = delegate()
                        {
                            PlayerManager pm = Costam.PlayerManager;
                            Player pl = Costam.PlayerManager.players.Values.ElementAt(0);
                            pl.ShootingEnabled = true;

                            l3.objectManager.InitDiamondSequences();
                            l3.objectManager.InitBloodSequences();
                            l3.objectManager.VirusGenerator.Enabled = true;
                            Trigger lessonThreeTrigger = new Trigger();
                            lessonThreeTrigger.Condition = delegate()
                            {
                                return (Costam.ColisionManager.virusesKilled >= killViruses);
                            };
                            lessonThreeTrigger.Action = delegate()
                            {
                                lessonThreeTrigger.Enabled = false;

                                ScreenManager.AddScreen(new GameOverScreen(pl.score, "Great! You have finished all lessons", false,3), ControllingPlayer);
                            };

                            Costam.TriggerManager.AddTrigger(lessonThreeTrigger);

                        };


                        LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, l3);

                    }
                    break;

            }
        }

        #endregion


        #region Update & Draw


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //lesson rectangle
            int x = titleRectangle.X;
            int y = titleRectangle.Y + titleRectangle.Height + screenOffset;
            int width = titleRectangle.Width;
            int height = viewport.Height - y - 3 * GetHeight() - screenOffset;

            Rectangle lessonRectangle = new Rectangle(x, y, width, height);

            spriteBatch.Draw(Costam.redFolder, lessonRectangle, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            Vector2 titlePos = new Vector2(viewport.Width / 2 - GetWidth(title) / 2, lessonRectangle.Y);

            spriteBatch.DrawString(font, title, titlePos, Color.Gold * TransitionAlpha, 0, vector00, 1f, SpriteEffects.None, 0);

            x = lessonRectangle.X + screenOffset;
            y = (int)titlePos.Y + GetHeight() + screenOffset;

            switch (lesson)
            {
                case 1:
                    DrawCentered(spriteBatch, "Welcome to the first lesson!", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "You will learn how to move in vein.", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "Try to move around and earn 150 points.", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "You can find instructions in the main menu.", y);


                    break;
                case 2:
                    DrawCentered(spriteBatch, "This is second lesson so far!", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "Now you will meet Red Blood Cell.", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "Try to collide with that cell.", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "You will lose one life.", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "Why ? Because that cells are our friends !", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "Then collect 150 coins.", y);
                    break;
                case 3:
                    DrawCentered(spriteBatch, "Third lesson. Be careful!", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "Time for your first enemy.", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "The virus take your life same as blood cell.", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "But you can destroy them with ALT attack.", y);
                    y += GetHeight();
                    DrawCentered(spriteBatch, "Kill 10 viruses to win.", y);


                    break;
            }

            // Draw each menu entry in turn.

            UpdateMenuEntryLocations(lessonRectangle.Y + lessonRectangle.Height + GetHeight());

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }
        }

        void DrawCentered(SpriteBatch spriteBatch, string text, int y)
        {
            SpriteFont font = ScreenManager.Font;
            spriteBatch.DrawString(font, text, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - GetWidth(text) / 2, y), Color.Gold * TransitionAlpha, 0, vector00, 1f, SpriteEffects.None, 0);
        }


        #endregion

    }
}
