using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Jitter.Collision;
using LifeFall.Logic;
using Jitter.Dynamics;
using Jitter.LinearMath;
using LifeFall.Logic.Blood;
using System;


namespace LifeFall.Core
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CollisionManager : Microsoft.Xna.Framework.GameComponent
    {
        CollisionSystem collisionSystem;
        CollisionSystem bulletCollisionSystem;
        PlayerManager playerManager;
        ObjectManager objectManager;

        public CollisionManager(Game game)//, PlayerManager playerManager, ObjectManager objectManager)
            : base(game)
        {
            //this.playerManager = playerManager;
            //this.objectManager = objectManager;
            playerManager = Costam.PlayerManager;
            objectManager = Costam.ObjectManager;

            collisionSystem = new CollisionSystemSAP();
            bulletCollisionSystem = new CollisionSystemSAP();

            collisionSystem.CollisionDetected += new CollisionDetectedHandler(CollisionDetected);
            bulletCollisionSystem.CollisionDetected += new CollisionDetectedHandler(BulletCollisionDetected);
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        public void TurnOffCollision()
        {
            collisionSystem.CollisionDetected -= CollisionDetected;
        }

        public void TurnOnCollision()
        {
            collisionSystem.CollisionDetected += new CollisionDetectedHandler(CollisionDetected);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //DetectCollisionsWithPlayers();

            base.Update(gameTime);
        }

        private void DetectCollisionsWithPlayers()
        {
            foreach (Player player in playerManager.players.Values)
            {
                //po obiektach
                foreach (GameObject obstacle in objectManager.Obstacles.Values)
                {
                    collisionSystem.Detect(obstacle.CModel.PhysicsModel.RigidBody, player.CModel.PhysicsModel.RigidBody);
                }

            }

        }

        public void CollisionManagerDetect(Player player, MovableObject obstacle)
        {

            collisionSystem.Detect(obstacle.CModel.PhysicsModel.RigidBody, player.CModel.PhysicsModel.RigidBody);
        }

        public void DetectShots(MovableObject bullet, MovableObject obstacle)
        {
            bulletCollisionSystem.Detect(obstacle.CModel.PhysicsModel.RigidBody, bullet.CModel.PhysicsModel.RigidBody);
        }

        public void CollisionDetected(RigidBody body1, RigidBody body2, JVector point1, JVector point2, JVector normal, float penetration)
        {
            //obstacle
            MovableObject obstacle = objectManager.Obstacles[body1];

            //player
            Player player = playerManager.players[body2];
            
           


            obstacle.OnCollision(player);

            // TODO : czy napewno tutaj?
            player.OnCollision(obstacle);

            objectManager.MarkToDispose(obstacle);

            
      
        }

        public int virusesKilled = 0;
        public void BulletCollisionDetected(RigidBody body1, RigidBody body2, JVector point1, JVector point2, JVector normal, float penetration)
        {
            Costam.explosionEffect.Play();
            //bullet
            Bullet b = objectManager.Bullets[body2];


            //obst
            MovableObject obstacle = objectManager.Obstacles[body1];
            Type t = obstacle.GetType();
            if (t == typeof(Virus))
            {
                virusesKilled++;
            }

            obstacle.OnCollision(b);
            objectManager.MarkToDispose(obstacle);
            objectManager.MarkToDispose(b);
        }

    }
}
