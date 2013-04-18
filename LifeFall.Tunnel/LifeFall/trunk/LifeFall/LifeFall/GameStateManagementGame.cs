using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeFall.Core;
using LifeFall.Core.Trigger_System;
using LifeFall.Logic;
using LifeFall.Debug;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace LifeFall
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class GameStateManagementGame : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;

        #region Fields

        public ScreenManager screenManager;


        // By preloading any assets used by UI rendering, we avoid framerate glitches
        // when they suddenly need to be loaded in the middle of a menu transition.
        static readonly string[] preloadAssets =
        {

        };


        #endregion

        #region Initialization


        /// <summary>
        /// The main game constructor.
        /// </summary>
        public GameStateManagementGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen =  Costam.IS_FULL_SCREEN_MODE;

            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;

            Costam.Game = this;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.          

            //screenManager.AddScreen(new CustomBackgroundScreen("ceiling_decoration_1 - Kopia"), null);
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

        Texture2D backgroundTexture;
        /// <summary>
        /// Loads graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            foreach (string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }

            Costam.BloodCellModel = Utils.LoadModel("blood_cell", Costam.RED_BLOOD_CELL_SIZE);
            Costam.BloodCellTexture = Content.Load<Texture2D>("gradient");
            Costam.VirusModel = Utils.LoadModel("virus_1", Costam.VIRUS_SIZE);
            Costam.DiamondModel = Utils.LoadModel("diamond", Costam.DIAMOND_SIZE);
            Costam.PlayerModel = Utils.LoadModel("P", Costam.PLAYER_SIZE);
            Costam.BulletModel = Utils.LoadModel("Bullet", Costam.BULLET_SIZE);
            Costam.goldDiamondTexture = Content.Load<Texture2D>("goldDiamond");
            Costam.redDiamondTexture = Content.Load<Texture2D>("redDiamond");
            Costam.greenDiamondTexture = Content.Load<Texture2D>("greenDiamond");

            Costam.playerTexture = Content.Load<Texture2D>("wedge_p1_diff_v1");

            Costam.oneLife = Content.Load<Texture2D>("healthbar1");
            Costam.twoLife = Content.Load<Texture2D>("healthbar2");
            Costam.threeLife = Content.Load<Texture2D>("healthbar3");
            Costam.noLife = Content.Load<Texture2D>("healthbar0");

            Costam.scoreBar = Content.Load<Texture2D>("scorebar");
            Costam.background = Content.Load<Texture2D>("redFolder");
            Costam.redFolder = Content.Load<Texture2D>("redFolder");
            Costam.menuBorder = Content.Load<Texture2D>("goldBorder");
            Costam.title = Content.Load<Texture2D>("title");

            Costam.bloodCell2D = Content.Load<Texture2D>("krwinka");
            Costam.virus2D = Content.Load<Texture2D>("wirus");
            Costam.goldDiamond2D = Content.Load<Texture2D>("zoltydiament");
            Costam.blueDiamond2D = Content.Load<Texture2D>("niebieskidiament");
            Costam.greemDiamond2D = Content.Load<Texture2D>("zielonydiament");

            Costam.A = Content.Load<Texture2D>("large_face_a");
            Costam.B = Content.Load<Texture2D>("large_face_b");
            Costam.DPAD_LR = Content.Load<Texture2D>("large_dpad_left_right");

            Costam.ARROWS = Content.Load<Texture2D>("keyboardMove");
            Costam.SPACE_ALT = Content.Load<Texture2D>("keyboard_space_alt");

            Costam.tutorialObjectsInterfejs = Content.Load<Texture2D>("interfejsInvisible");

            Costam.menuSong = Content.Load<Song>("DST-BloodAndIron");
            Costam.gameSong = Content.Load<Song>("DST-Psykick");
        }


        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            //graphics.GraphicsDevice.Clear(new Color(51,51,51));

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }


        #endregion
    }
}
