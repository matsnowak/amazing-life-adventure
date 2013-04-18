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
using LifeFall.Core.Controls;
using LifeFall.Logic;
using LifeFall.Logic.Blood;


namespace LifeFall.Core
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : MovableObject
    {
        public float rotationPerSecond = MathHelper.TwoPi / 2.0f;
        public float changingRadiusAngleDelta = MathHelper.TwoPi / 36.0f;

        public Player()
            : base(Costam.PlayerModel)
        {

            health = 3;
            score = 0;
            initBehaviours();
            BoundingSphere bs = (BoundingSphere)CModel.Model.Tag;
            bs.Radius = Costam.PlayerBoundingSphereRadius;
            CModel.Model.Tag = bs;
        }

        public Player(Model model)
            : base(model)
        {
            health = 3;
            score = 0;
            initBehaviours();
            BoundingSphere bs = (BoundingSphere)CModel.Model.Tag;
            bs.Radius = Costam.PlayerBoundingSphereRadius;
            CModel.Model.Tag = bs;
        }

        #region GameFields

        public string nickName;
        public float health;
        public int score;

        public IControler controler;
        public IControler padControler;
        public IControler kinectControler;

        #endregion GameFields

        #region Fields

        #endregion Fields

        public bool ShootingEnabled = true;
        public override void OnCollision(Player player)
        {
            throw new NotImplementedException();
        }

        public override void OnCollision(Bullet b)
        {
            
        }

        public bool CollisionWithRedBLoodCellHappened = false;
        public virtual void OnCollision(MovableObject movableObject)
        {
            Type t = movableObject.GetType();
            if (t == typeof(RedBloodCell))
            {
                CollisionWithRedBLoodCellHappened = true;
            }
        }

        public void EnableGodMode()
        {
            AddBehaviour(godMode);
        }

        public override void Update(GameTime gameTime)
        {
            if (controler.Left() || padControler.Left())
            {
                this.Angle += (float)gameTime.ElapsedGameTime.TotalSeconds * rotationPerSecond;
            }

            if (controler.Right() || padControler.Right())
            {
                this.Angle -= (float)gameTime.ElapsedGameTime.TotalSeconds * rotationPerSecond;
            }

            if (controler.Up() || padControler.Up())
            {
                AddBehaviour(radiusDecreasing);
            }
            if (controler.Down() || padControler.Down())
            {
                AddBehaviour(radiusIncreasing);
            }

            if (controler.Action())
            {
                if (ShootingEnabled)
                {
                    KillItWihtFire();
                }
            }
            base.Update(gameTime);
        }

        Behaviour radiusDecreasing;
        Behaviour radiusIncreasing;
        Behaviour godMode;
        float radiusChangingSpeed = 80; // per second

        float godModeTimeFirst  = 1;
        float godModeTimeSecond = 1;
        float maxAlpha          = 1f;
        float minAlpha          = 0.1f;
        float timeCounter       = 0;
        int direction           = -1;
        int phase               = 1;
        float ad                = 6;

        public void KillItWihtFire()
        {
            Costam.fireEffect.Play();
            Bullet b = Costam.MemoryPoolObjectProvider.GetObject<Bullet>();
            b.Fire(this, Forward);
            Costam.ObjectManager.AddBullet(b);
        }

        private void initBehaviours()
        {
            radiusDecreasing = new Behaviour();
            radiusDecreasing.Init((MovableObject)(this));
            this.AutoRotate = true;
            radiusDecreasing.Execute = delegate(GameTime gameTime)
            {
                RemoveBahaviour(radiusIncreasing);
                if (radiusDecreasing.Owner.Radius > PlayerManager.SmallRadius)
                {// jescze trwa zmiana wysokoœci
                    float radiusDelta = radiusChangingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    radiusDecreasing.Owner.Radius -= radiusDelta;
                    if (radiusDecreasing.Owner.Radius < PlayerManager.SmallRadius)
                    {
                        radiusDecreasing.Owner.Radius = PlayerManager.SmallRadius;
                    }
                }
                else
                {
                    RemoveBahaviour(radiusDecreasing);
                }
            };
            radiusIncreasing = new Behaviour();
            radiusIncreasing.Init((MovableObject)(this));
            this.AutoRotate = true;
            radiusIncreasing.Execute = delegate(GameTime gameTime)
            {
                RemoveBahaviour(radiusDecreasing);
                if (radiusIncreasing.Owner.Radius < PlayerManager.BigRadius)
                {// jescze trwa zmiana wysokoœci
                    float radiusDelta = radiusChangingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    radiusIncreasing.Owner.Radius += radiusDelta;
                    if (radiusIncreasing.Owner.Radius > PlayerManager.BigRadius)
                    {
                        radiusIncreasing.Owner.Radius = PlayerManager.BigRadius;
                    }
                }
                else
                {
                    RemoveBahaviour(radiusIncreasing);
                }
            };

            godMode = new Behaviour();
            godMode.Init((MovableObject)(this));
            godMode.Execute = delegate(GameTime gameTime)
            {
                // 1 faza
                if (phase == 1)
                {
                    Costam.ColisionManager.TurnOffCollision();
                    timeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    float deltaAplpha = (float)gameTime.ElapsedGameTime.TotalSeconds * ad / godModeTimeFirst;

                    foreach (ModelMesh mesh in CModel.Model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {

                            effect.Alpha += deltaAplpha * direction;
                            if (effect.Alpha > maxAlpha)
                            {
                                effect.Alpha -= effect.Alpha - maxAlpha;
                                direction *= -1;
                            }

                            if (effect.Alpha < minAlpha)
                            {
                                effect.Alpha -= effect.Alpha - minAlpha;
                                direction *= -1;
                            }
                        }

                    }
                    if (timeCounter > godModeTimeFirst)
                    {
                        timeCounter = 0;
                        phase = 2;

                    }

                }
                if (phase == 2)
                {
                    foreach (ModelMesh mesh in CModel.Model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.Alpha = 1;
                        }
                    }
                    Costam.ColisionManager.TurnOnCollision();
                    RemoveBahaviour(godMode);
                    phase = 1;
                }
            };
        }

    }
}