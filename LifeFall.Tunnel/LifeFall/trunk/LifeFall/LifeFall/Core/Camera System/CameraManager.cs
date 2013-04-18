using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace LifeFall.Core
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CameraManager : Microsoft.Xna.Framework.GameComponent
    {

        Dictionary<string, ICamera> cameras;
        ICamera activeCamera;

        public CameraManager(Game game)
            : base(game)
        {
            cameras = new Dictionary<string, ICamera>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
#if DEBUG
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.D1))
            {
                SetActiveCamera("ChaseCamera");
            }
            if (ks.IsKeyDown(Keys.D2))
            {
                SetActiveCamera("DebugCamera");
                GetCamera("DebugCamera").Position = GetCamera("ChaseCamera").Position;
            }
#endif

            (activeCamera as GameComponent).Update(gameTime);

            base.Update(gameTime);
        }
        /// <summary>
        /// Dodaje kamere. Wywo³uje camera.Initalize() i ustawia camera na aktualn¹ kamere.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="camera"></param>
        public void AddCamera(string Name, ICamera camera)
        {
            cameras.Add(Name, camera);
            (camera as GameComponent).Initialize();
            SetActiveCamera(Name);
        }

        public ICamera GetCamera(string Name)
        {
            return cameras[Name];
        }

        public void RemoveCamera(string Name)
        {
            cameras.Remove(Name); 
        }

        public ICamera GetActiveCamera()
        {
            return activeCamera;
        }

        public void SetActiveCamera(String Name)
        {
            activeCamera = cameras[Name];
            
        }

    }
}
