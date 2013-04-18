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
using Jitter.LinearMath;
using LifeFall.Core.Controls;
using Jitter.Dynamics;


namespace LifeFall.Core
{
    public class PlayerManager 
    {
        //public List<Player> players;
        public Dictionary<RigidBody, Player> players;

        public static float speed = 350;
        private float deltaSpeed = 10;
        private float maxSpeed = 500;
        public static float rotationPerSecond = MathHelper.TwoPi / 2.0f;

        public static float BigRadius = Costam.TUNNEL_RADIUS - 5;
        public  static  float SmallRadius = BigRadius * 0.6f;

        public float movingSpeed = 20f;
       

        public PlayerManager(Game game)
        {
            //players = new List<Player>();
            players = new Dictionary<RigidBody, Player>();

            InitializePlayers();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
#if DEBUG
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.OemPlus))
            {
                speed += deltaSpeed;
            }

            if (ks.IsKeyDown(Keys.OemMinus))
            {
                speed -= deltaSpeed;
                
            }
            if (ks.IsKeyDown(Keys.D0))
            {
                speed = 0;
            }

#endif


            foreach (Player P in players.Values)
            {
                P.Speed = speed;
                P.controler.Update();
                P.padControler.Update();
            }

            foreach (Player P in players.Values)
            {
                UpdatePlayer(P, gameTime);
            }

        }


        public void Draw(GameTime gameTime)
        {
            foreach (Player P in players.Values)
            {
                P.Draw();
#if DEBUG
                Costam.DebugWrite("");
                Costam.DebugWrite("Player.speed : " + speed.ToString());

                Costam.DebugWrite("Player.Postion.X : " + P.Position.X);
                Costam.DebugWrite("Player.Postion.Y : " + P.Position.Y);
                Costam.DebugWrite("Player.Postion.Z : " + P.Position.Z);

                //Costam.DebugWrite("");
                //Costam.DebugWrite("Up.X : " + P.UpVector.X.ToString());
                //Costam.DebugWrite("Up.Y : " + P.UpVector.Y.ToString());
                //Costam.DebugWrite("Up.Z : " + P.UpVector.Z.ToString());
                //Costam.DebugWrite("");
                //Costam.DebugWrite("Forward.X : " + P.Forward.X.ToString());
                //Costam.DebugWrite("Forward.Y : " + P.Forward.X.ToString());
                //Costam.DebugWrite("Forward.Z : " + P.Forward.X.ToString());

                //Costam.DebugWrite("");
                //Costam.DebugWrite("Angle : " + P.Angle.ToString());
                //Costam.DebugWrite("Radius : " + P.Radius.ToString());
#endif

            }

        }


        private void InitPlayer(Player player, float angle)
        {
            player.Radius = BigRadius;
            player.TunnelPosition = player.Track.ElementAt(5);
            player.Angle = angle;
            player.Speed = speed;
            player.CModel.textureEnabled = true;
            player.CModel.texture = Costam.playerTexture;
            AddPlayer(player);
        }

        private void AddPlayer(Player player)
        {
            players.Add(player.CModel.PhysicsModel.RigidBody, player);
        }


        private void InitializePlayers()
        {
            Player player1 = new Player();
            player1.Track = Costam.TunnelManager.tp.PathPoints;

            IControler keyboardController = new KeyboardControler();
            keyboardController.LoadSettings(Keys.Left, Keys.Right, Keys.Up, Keys.Down);
            IControler padControlle = new XboxPadControler(PlayerIndex.One);
            player1.controler = keyboardController;
            player1.padControler = padControlle;

            player1.nickName = "rexilion";
            InitPlayer(player1, MathHelper.Pi);


            ////second player
        
            ////Player player2 = new Player();
            ////player2.CModel.Scale = new Vector3(2f, 2f, 2f);

            //player2.Track = Costam.TunnelManager.tp.PathPoints;

            //IControler keyboardController2 = new KeyboardControler();
            //keyboardController2.LoadSettings(Keys.NumPad4, Keys.NumPad6, Keys.NumPad8, Keys.NumPad5);
            //player2.controler = keyboardController2;

            //player2.nickName = "mats";
            //InitPlayer(player2, MathHelper.PiOver2);

        }

        #region CoreStuff
        private void UpdatePlayer(Player player, GameTime gameTime)
        {
            //if (player.controler.Left() || player.padControler.Left())
            //{
            //    player.Angle += (float)gameTime.ElapsedGameTime.TotalSeconds * rotationPerSecond;
            //}

            //if (player.controler.Right() || player.padControler.Right())
            //{
            //    player.Angle -= (float)gameTime.ElapsedGameTime.TotalSeconds * rotationPerSecond;
            //}

            //if (player.controler.Up())
            //{

            //}
            //if (player.controler.Down())
            //{

            //}

            //if (player.controler.Up())
            //{
            //    //speed += deltaSpeed;
            //    //if (speed > maxSpeed)
            //    //    speed = maxSpeed;
            //    //player.Jump();
            //}

            //if (player.controler.Down())
            //{
            //    //speed -= deltaSpeed;
            //    //if (speed < 0)
            //    //    speed = 0;
            //    player.Crouch();
            //}else
            //{
            //    player.Stand();
            //} 

            //player.UpdateJump((float)gameTime.ElapsedGameTime.TotalSeconds * movingSpeed);

            //player.Move((float)gameTime.ElapsedGameTime.TotalSeconds * speed);
            player.Update(gameTime);

        }
        #endregion CoreStuff
    }
}
