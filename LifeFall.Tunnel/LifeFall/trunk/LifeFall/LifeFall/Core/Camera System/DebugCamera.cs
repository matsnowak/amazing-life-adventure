

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace LifeFall.Core
{
    public class DebugCamera : GameComponent, ICamera
    {
        public Vector3 Position { get; set;}
        public Vector3 Forward { get; set;}
        public Vector3 UpVector { get; set;}
        
        public Matrix ViewMatrix { get; set;}
        public Matrix ProjectionMatrix { get; set;}

        Viewport viewPort;

        // Speed
        float directionSpeed = 0.00080f;
        float rotationSpeed = -MathHelper.PiOver4 / 10000;

        // Mouse stuff
        MouseState originalMouseState;

        public DebugCamera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Build camera view matrix
            Position = pos;
            Forward = target - pos;
            Forward.Normalize();
            UpVector = up;
            CreateLookAt();

            viewPort = Game.GraphicsDevice.Viewport;

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                1, 5000);
        }

        public override void Initialize()
        {
            // Set mouse position and do initial get state
            Mouse.SetPosition(viewPort.Width / 2,
                viewPort.Height / 2);
            originalMouseState = Mouse.GetState();

            //RasterizerState wireFrameState = new RasterizerState()
            //{
            //    FillMode = FillMode.WireFrame,
            //    CullMode = CullMode.CullClockwiseFace,
            //};
            //Game.GraphicsDevice.RasterizerState = wireFrameState;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            MouseState currentMouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            float deltaDirection = directionSpeed * gameTime.ElapsedGameTime.Ticks / 1000;

            // Move forward/backward
            if (keyboardState.IsKeyDown(Keys.W))
                Position += Forward * deltaDirection;
            if (keyboardState.IsKeyDown(Keys.S))
                Position -= Forward * deltaDirection
            ;
            // Move side to side
            if (keyboardState.IsKeyDown(Keys.A))
                Position += Vector3.Cross(UpVector, Forward) * deltaDirection;
            if (keyboardState.IsKeyDown(Keys.D))
                Position -= Vector3.Cross(UpVector, Forward) * deltaDirection;


            float xDelta = currentMouseState.X - originalMouseState.X;
            float yDelta = currentMouseState.Y - originalMouseState.Y;

            //// Yaw rotation
            Forward = Vector3.Transform(Forward,
                Matrix.CreateFromAxisAngle(UpVector, 6 * rotationSpeed *
                xDelta));


            // Pitch rotation
            Forward = Vector3.Transform(Forward,
                Matrix.CreateFromAxisAngle(Vector3.Cross(UpVector, Forward),
                rotationSpeed * -yDelta));

            UpVector = Vector3.Transform(UpVector,
                Matrix.CreateFromAxisAngle(Vector3.Cross(UpVector, Forward),
               rotationSpeed * -yDelta));

            // Reset prevMouseState
            Mouse.SetPosition(viewPort.Width / 2, viewPort.Height / 2);


            // Recreate the camera view matrix
            CreateLookAt();

            base.Update(gameTime);
        }

        private void CreateLookAt()
        {
            ViewMatrix = Matrix.CreateLookAt(Position,
                Position + Forward, UpVector);
        }


    }
}


