//#define DRAW_LINES_TO_OBJECTS


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
using LifeFall.Logic.Blood;
using LifeFall.Core;
using Jitter.LinearMath;
using Jitter.Dynamics;
using LifeFall.Logic.Sequences;
using LifeFall.Logic.Sequences.Base;
using LifeFall.Logic.Behaviours;



namespace LifeFall.Logic
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ObjectManager : DrawableGameComponent
    {
        //public List<MovableObject> Obstacles;

        public Dictionary<RigidBody, MovableObject> Obstacles;
        public SortedList<float, Sequence> Sequences;
        public List<FastSequence> NonSortedSequences;
        public List<MovableObject> ToDispose;

        private TunnelManager tunnelManager;

        int InitSize = 0;
        float distanceFromEdge = 3;

        public float VirusRadius;
        public float CollectiblePointRadius;
        public float RedBloodCellRadius;
        public float DiamondRadius;

        public float DifficultRatio = 1.0f;
        float ratioRadius = 0;
        float ratioDelta = 0;

        float OneLevelProgressTimeInSecond = 85;

        float distanceBetweenObstacles = 500;
        float distanceBeetwenObstaclesRadius = 150;

        MovableObject respawnPoint;
        MovableObject respawnPointSentinel;
        float respawnPointSentinellDistance = 3500;

        int startingPointRespawnTunnelPathIndex = 10;

        float nonCollisionRadius = Costam.TUNNEL_RADIUS / 2.0f;
        FastSequence newSequence;
        Random random;

        /*  BLOOD  */
        public ObstaclesGenerator<RedBloodCell> BloodStreamGenerator;
        // Iloœæ krwinek na bloodStreamPartSize jednostek
        int bloodCellDensity = 10;
        float bloodCellStreamSize = 1000;
        // Przyspieszenie krwi przy skurczu
        float heartContractionInterval = 60.0f / Costam.BPM;


        /* VIRUSES */
        public ObstaclesGenerator<Virus> VirusGenerator;
        int virusDensity = 10;
        float VirusStreamSize = 3000;

        #region Debug Tools

        #endregion


        public float CalculateRadius(float tunnelRadius, float objectDiagonal)
        {
            return tunnelRadius - (objectDiagonal / 2.0f) - distanceFromEdge;
        }

        public ObjectManager(Game game)
            : base(game)
        {

            random = new Random((int)System.DateTime.Now.Ticks);
            tunnelManager = Costam.TunnelManager;

            //Obstacles = new List<MovableObject>();
            Obstacles = new Dictionary<RigidBody, MovableObject>();
            Sequences = new SortedList<float, Sequence>();
            NonSortedSequences = new List<FastSequence>();

            ToDispose = new List<MovableObject>();

            respawnPoint = new CollectiblePoint();
            respawnPoint.TunnelPosition = Costam.TunnelManager.tp.PathPoints[startingPointRespawnTunnelPathIndex];
            respawnPointSentinel = new CollectiblePoint();
            respawnPointSentinel.TunnelPosition = Costam.TunnelManager.tp.PathPoints[startingPointRespawnTunnelPathIndex];
            respawnPointSentinel.Move(respawnPointSentinellDistance);

            VirusRadius = CalculateRadius(Costam.TUNNEL_RADIUS, Costam.VIRUS_SIZE);
            CollectiblePointRadius = CalculateRadius(Costam.TUNNEL_RADIUS, Costam.COLLECTIBLE_POINT_SIZE);
            RedBloodCellRadius = CalculateRadius(Costam.TUNNEL_RADIUS, Costam.RED_BLOOD_CELL_SIZE);
            DiamondRadius = CalculateRadius(Costam.TUNNEL_RADIUS, Costam.DIAMOND_SIZE);

            //InitBloodSequences(); InitDiamondSequences(); InitVirusesSequences();
            //InitDiamondSequences();
            //newSequenceIndex = random.Next(0, NonSortedSequences.Count); // TODO : zmieniæ kod przy zmianie na dictionary
            //newSequence = NonSortedSequences[newSequenceIndex];

            #region Blood Stream
            BloodStreamGenerator = new ObstaclesGenerator<RedBloodCell>();
            BloodStreamGenerator.TrackedObject = Costam.PlayerManager.players.Values.ElementAt(0);
            BloodStreamGenerator.Density = bloodCellDensity;
            BloodStreamGenerator.DistanceToTrackedObject = 1000;
            BloodStreamGenerator.PartLength = bloodCellStreamSize;
            BloodStreamGenerator.MinRadius = 0.1f;
            BloodStreamGenerator.MaxRadius = Costam.TUNNEL_RADIUS / 3;
            BloodStreamGenerator.MinAngle = 0;
            BloodStreamGenerator.MaxAngle = MathHelper.TwoPi;
            BloodStreamGenerator.ObjectBehaviours.Add(new BloodStream());

            BloodStreamGenerator.Enabled = true;
            #endregion BloodStream

            #region Viruses
            VirusGenerator = new ObstaclesGenerator<Virus>()
            {
                TrackedObject = Costam.PlayerManager.players.Values.ElementAt(0),
                Density = virusDensity,
                DistanceToTrackedObject = 1500,
                PartLength = VirusStreamSize,
                MinRadius = PlayerManager.SmallRadius,
                MaxRadius = VirusRadius,
                MinAngle = 0,
                MaxAngle = MathHelper.TwoPi,
            };
            VirusGenerator.Enabled = true;

            #endregion Viruses


        }

        void AddSequence(FastSequence s)
        {
            NonSortedSequences.Add(s);
        }

        public void initSeqSetup()
        {
            newSequenceIndex = random.Next(0, NonSortedSequences.Count); // TODO : zmieniæ kod przy zmianie na dictionary
            newSequence = NonSortedSequences[newSequenceIndex];
        }

        public void InitVirusesSequences()
        {
            FastSequence VirusHelix1L = new Helix<Virus>(8, MathHelper.PiOver2, DiamondRadius, 500, TurningDirection.Left, 2);
            AddSequence(VirusHelix1L);
            initSeqSetup();
        }

        public void InitBloodSequences()
        {
            FastSequence BloodCellHelix1L = new Helix<RedBloodCell>(8, MathHelper.Pi, RedBloodCellRadius, 900, TurningDirection.Left, 2);
            AddSequence(BloodCellHelix1L);

            FastSequence BloodCellHelix1R = new Helix<RedBloodCell>(10, MathHelper.Pi, RedBloodCellRadius, 900, TurningDirection.Right, 2);
            AddSequence(BloodCellHelix1R);
            initSeqSetup();
        }

        public void InitDiamondSequences()
        {
            FastSequence VirusRing1 = new Ring<GoldDiamond>(3, DiamondRadius);
            AddSequence(VirusRing1);

            FastSequence VirusRing2 = new Ring<GoldDiamond>(6, DiamondRadius);
            AddSequence(VirusRing2);

            FastSequence bloodcellring = new Ring<RedDiamond>(4, DiamondRadius);
            AddSequence(bloodcellring);

            

            FastSequence VirusHelix1R = new Helix<RedDiamond>(8, MathHelper.PiOver2, DiamondRadius, 500, TurningDirection.Right, 2);
            AddSequence(VirusHelix1R);

            FastSequence VirusHelix2L = new Helix<GoldDiamond>(6, MathHelper.PiOver2 / 2, DiamondRadius, 350, TurningDirection.Left, 4);
            AddSequence(VirusHelix2L);

            FastSequence VirusHelix2R = new Helix<RedDiamond>(6, MathHelper.PiOver2 / 2, RedBloodCellRadius, 500, TurningDirection.Right, 3);
            AddSequence(VirusHelix2R);

            initSeqSetup();
        }


        public void lessonOne()
        {
            InitDiamondSequences();
            VirusGenerator.Enabled = false;
            BloodStreamGenerator.Enabled = true;
        }



        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
         public override void Initialize()
        {


            // TODO : Dorobiæ klase która bêdzie reprezentowaæ tylko punkty w tunelu, nie s³u¿¹ce do rysowania


        }

        int newSequenceIndex;


        // TODO: Zadbaæ o to, ¿eby ¿adna sekwencja nie by³a d³u¿sza ni¿ 3/4 tunelu
        void fillTunnelObstacles()
        {


#if DEBUG
            //Costam.DebugWrite("RespawnPoint.X: " + respawnPoint.TunnelPosition.X.ToString());
            //Costam.DebugWrite("RespawnWall.X: " + Costam.TunnelManager.tp.PathPoints[respawnTunnelPathPointIndex].X);
#endif

            while (respawnPoint.TunnelPosition.X + newSequence.GetCurrentLength() < respawnPointSentinel.TunnelPosition.X)
            {
                float range = DifficultRatio - 1;       // Losujemy zakres tak, ¿eby ratio nie moglo spaœæ poni¿ej 1;
                ratioRadius = (float)random.NextDouble() * 2 * range; // Losujemy mo¿liw¹ najwiêksz¹ rozbie¿noœc od aktualnego DifficultRatio
                ratioRadius -= range;
                ratioDelta = (float)random.NextDouble() * ratioRadius; // Losujemy aktualne odchylenie;

                newSequence.BalanceDifficulty(DifficultRatio + ratioDelta);
                newSequence.ConstructOnPosition(respawnPoint.TunnelPosition);

                AddObstacles(newSequence.GetObjectList());

                float distanceDelta = (float)random.NextDouble() * 2 * distanceBeetwenObstaclesRadius;
                distanceDelta -= distanceBeetwenObstaclesRadius;

                respawnPoint.Move(newSequence.GetCurrentLength() + distanceBetweenObstacles + distanceDelta);

                newSequenceIndex = random.Next(0, NonSortedSequences.Count); // TODO : zmieniæ kod przy zmianie na dictionary

                newSequence = NonSortedSequences[newSequenceIndex];
            }
            respawnPointSentinel.TunnelPosition = Costam.PlayerManager.players.Values.ElementAt(0).TunnelPosition;
            respawnPointSentinel.Move(respawnPointSentinellDistance);
        }




        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            calculateBloodSpeed(gameTime);

            #region Generators
            BloodStreamGenerator.Update(gameTime);
            stealObjects(BloodStreamGenerator.GetElements());

            // VIRUSES //
            VirusGenerator.Update(gameTime);
            stealObjects(VirusGenerator.GetElements());



            #endregion

            //Player player = Costam.PlayerManager.players.Values.ElementAt(0);
            foreach (MovableObject obstacle in Obstacles.Values)
            {
                obstacle.Update(gameTime);
                foreach (Player player in Costam.PlayerManager.players.Values)
                {
                    Costam.ColisionManager.CollisionManagerDetect(player, obstacle);

                    if (obstacle.Position.X < player.Position.X - 100)
                    {
                        MarkToDispose(obstacle);
                    }
                }


            }

            foreach (Bullet bu in Bullets.Values)
            {
                bu.Update(gameTime);
                foreach (MovableObject o in Obstacles.Values)
                {
                    Costam.ColisionManager.DetectShots(bu, o);
                }


            }

            #region UpdateProgress

            float progressDelta = (float)(1 / OneLevelProgressTimeInSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            DifficultRatio += progressDelta;

            #endregion UpdateProgress


            DisposeNonVisibleObstacles();
            fillTunnelObstacles();


            fillTunnelObstacles();
            //base.Update(gameTime);
        }

        private void stealObjects(List<MovableObject> objects)
        {
            foreach (MovableObject o in objects)
            {
                Obstacles.Add(o.CModel.PhysicsModel.RigidBody, o);
            }
        }

        public Dictionary<RigidBody, Bullet> Bullets = new Dictionary<RigidBody, Bullet>();
        public void AddBullet(Bullet bullet)
        {
            if (!Bullets.Values.Contains(bullet)){
                Bullets.Add(bullet.CModel.PhysicsModel.RigidBody, bullet);
            }

        }

        public void RemoveBullet(MovableObject bullet)
        {

            Bullets.Remove(bullet.CModel.PhysicsModel.RigidBody);

        }

        float[] heartSpeedY = { 1.0f, 2.5f, 6.0f, 2.5f, 1.0f };
        float[] heartSpeedX = { 0.0f, 0.15f, 0.3f, 0.45f, 0.6f };

        float heartSpeedCycleTime = 0;
        //float heartSpeedCycleElapsedTime = 0;
        void calculateBloodSpeed(GameTime gameTime)
        {
            heartContractionInterval = 60.0f / Costam.BPM;
            heartSpeedCycleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (heartSpeedCycleTime > heartContractionInterval)
            {
                heartSpeedCycleTime -= heartContractionInterval;
            }

            int speedSelector = (int)(heartSpeedCycleTime / 0.15f);
            float speedMultiplier = 1;
            if (speedSelector < 4)
            {
                Single amount = heartSpeedCycleTime - (speedSelector * 0.15f);
                speedMultiplier = heartSpeedY[speedSelector] + (heartSpeedY[speedSelector + 1] - heartSpeedY[speedSelector]) * (amount);
            }

            Costam.BLOOD_SPEED = speedMultiplier * Costam.DEFAULT_BLOOD_SPEED;
        }

        public void MarkToDispose(MovableObject obstacle)
        {
            if (!ToDispose.Contains(obstacle))
            {
                ToDispose.Add(obstacle);
            }
        }

        private void DisposeNonVisibleObstacles()
        {
            for (int i = 0; i < ToDispose.Count; ++i)
            {
                Obstacles.Remove(ToDispose[i].CModel.PhysicsModel.RigidBody);
                Bullets.Remove(ToDispose[i].CModel.PhysicsModel.RigidBody);
                Costam.MemoryPoolObjectProvider.Dispose(ToDispose[i]);
            }
            ToDispose.Clear();

        }
        public void AddObstacles(List<MovableObject> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (Obstacles.Keys.Contains(list[i].CModel.PhysicsModel.RigidBody) == false)
                {
                    Obstacles.Add(list[i].CModel.PhysicsModel.RigidBody, list[i]);
                }
                else
                {
                    Costam.MemoryPoolObjectProvider.Dispose(list[i]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            Costam.DebugWrite("DifficultRatio: " + DifficultRatio.ToString());
#endif
            foreach (MovableObject obstacle in Obstacles.Values)
            {
                obstacle.Draw();
#if DEBUG && DRAW_LINES_TO_OBJECTS
                Costam.DebugDraw.DrawLine(Costam.PlayerManager.players.Values.ElementAt(0).Position, obstacle.Position);
#endif
            }

            foreach (Bullet b in Bullets.Values)
            {
                b.Draw();

            }

        }




        public int fillCounter = 0;
    }
}
