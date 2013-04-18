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
using Microsoft.Xna.Framework.Media;
using LifeFall.Core;
using System.Linq;
using LifeFall.Core.Trigger_System;
#endregion

namespace LifeFall
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    class GameOverScreen : MenuScreen
    {


        #region Initialization

        int points;
        string message;
        bool restart;
        int lesson;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GameOverScreen(int points,string message,bool restart,int lesson)
            : base("")
        {
            this.points = points;
            this.message = message;
            this.restart = restart;
            this.lesson = lesson;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            // Create our menu entries.
            if (restart)
            {
                MenuEntry restartGameMenuEntry = new MenuEntry("Restart Game");
                restartGameMenuEntry.Selected += RestartGameEntrySelected;
                MenuEntries.Add(restartGameMenuEntry);
            }

            
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit");

            // Hook up menu event handlers.
            
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.

            
            MenuEntries.Add(quitGameMenuEntry);

        }


        #endregion



        #region Handle Input


        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Play(Costam.menuSong);
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void RestartGameEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Play(Costam.gameSong);

            switch (lesson)
            {
                case 0:
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
                    break;
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

                                ScreenManager.AddScreen(new GameOverScreen(pl.score, "Excellent!", false, 1), ControllingPlayer);
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

        #region Update and Draw

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteFont font = ScreenManager.Font;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            string gameOver = message;

            Vector2 gameOverPos = new Vector2(viewport.Width / 2 - GetWidth(gameOver) / 2,viewport.Height/3);

            spriteBatch.DrawString(font, gameOver, gameOverPos, Color.Red * TransitionAlpha, 0, vector00, 1f, SpriteEffects.None, 0);

            string pointsLine = "Final score: " + points;
            Vector2 pointsPos = new Vector2(viewport.Width/2 - GetWidth(pointsLine)/2, gameOverPos.Y + GetHeight());
            spriteBatch.DrawString(font, pointsLine, pointsPos, Color.Gold, 0, vector00, 1f, SpriteEffects.None, 0);



            UpdateMenuEntryLocations((int)pointsPos.Y + 2 * GetHeight());

            // Draw each menu entry in turn.
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