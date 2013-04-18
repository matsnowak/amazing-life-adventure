using LifeFall.Core;
using LifeFall.Core.Trigger_System;
using LifeFall.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LifeFall.Logic;
using Microsoft.Xna.Framework.Graphics;
using LifeFall.Logic.Blood;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace LifeFall
{
    public static class Costam
    {
        #region Settings
        

        // TUNNEL PATH
        public const int    TUNNEL_PATH_TESSELLATION            = 50;
        public const int    TUNNEL_PATH_RADIUS                  = 120;
        public const int    TUNNEL_PATH_INTERVAL                = 500;
        public const int    TUNNEL_PATH_ONE_PART_SEGMENTS       = 6;

        // TUNNEL
        public const int    TUNNEL_TESSELLATION                 = 32;
        public const int    TUNNEL_RADIUS                       = 50;
        public const float  TUNNEL_TEXTURE_SCALE_X              = 1f;
        public const float  TUNNEL_TEXTURE_SCALE_Y              = 1f;

        // MODEL SIZES
        public const float  RED_BLOOD_CELL_SIZE                 = TUNNEL_RADIUS * 1.0f / 4.0f;
        public const float  VIRUS_SIZE                          = TUNNEL_RADIUS * 1.0f / 4.0f;
        public const float  COLLECTIBLE_POINT_SIZE              = TUNNEL_RADIUS * 1.0f / 4.0f;
        public const float  DIAMOND_SIZE                        = TUNNEL_RADIUS * 1.0f / 4.0f;
        public const float  BULLET_SIZE                         = TUNNEL_RADIUS * 1.0f / 4.0f;                                                                           
        public const float  PLAYER_SIZE                         = TUNNEL_RADIUS * 1.0f / 3.0f;

        // BLOOD
        public static float BPM                                 = 80;
        public static float BLOOD_SPEED                         = -100;
        public static float DEFAULT_BLOOD_SPEED                 = -300;
        public static float HEART_CONTRACTION_TIME              = 0.3f;

        #endregion

        #region Components
        public static CameraManager CameraManager;
        public static PlayerManager PlayerManager;
        public static TunnelManager TunnelManager;
        public static CollisionManager ColisionManager;
        public static TriggerManager TriggerManager;
        public static ObjectManager ObjectManager;

        public static SimpleMemoryPool MemoryPoolObjectProvider;
        public static GameStateManagementGame Game;
        public static Hud HUD;

        public static DebugDraw DebugDraw;

        public static Random Random;
        #endregion Components

        #region Models
        public static Model BloodCellModel;
        public static Model VirusModel;
        public static Model CollectiblePointModel;
        public static Model PlayerModel;
        public static Model DiamondModel;
        public static Model BulletModel;


        public static Texture2D BloodCellTexture;
        

        public static Texture2D goldDiamondTexture;
        public static Texture2D redDiamondTexture;
        public static Texture2D greenDiamondTexture;
        public static Texture2D playerTexture;
        

        public static Texture2D oneLife;
        public static Texture2D twoLife;
        public static Texture2D threeLife;
        public static Texture2D noLife;


        public static float PlayerBoundingSphereRadius = 4f;
        public static float VirusBoundingSphereRadius = 3f;
        public static float BloodCellBoundingSphereRadius = 3f;
        public static float DiamondBoundingSphereRadius = 3;
        public static float BulletBoundingSphereRadius = 6;
        #endregion Models

        #region Methods
        public static void DebugWrite(string Message)
        {
            DebugConsole debug = HUD.GetComponent(GameplayScreen.DebugDrawWindowName) as DebugConsole;
            if (debug != null)
            {
                debug.AddLine(Message);
            }
            else
            {
                new NullReferenceException("Brak podpiętego debugDrawa");
            }
        }

        #endregion Methods



        //GAME
        public static bool IS_FULL_SCREEN_MODE = true;

        #region UI

        public static Texture2D redFolder;
        public static Texture2D background;
        public static Texture2D menuBorder;
        public static Texture2D scoreBar;
        public static Texture2D title;


        public static Texture2D bloodCell2D;
        public static Texture2D virus2D;
        public static Texture2D goldDiamond2D;
        public static Texture2D blueDiamond2D;
        public static Texture2D greemDiamond2D;

        public static Texture2D A;
        public static Texture2D B;
        public static Texture2D DPAD_LR;
        public static Texture2D ARROWS;
        public static Texture2D SPACE_ALT;

        public static Texture2D tutorialObjectsInterfejs;

        #endregion UI

        public static Song menuSong;
        public static Song gameSong;

        public static SoundEffect collectEffect;
        public static SoundEffect clickEffect;
        public static SoundEffect collisionEffect;
        public static SoundEffect fireEffect;
        public static SoundEffect explosionEffect;

    }
}
