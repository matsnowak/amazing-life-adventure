using Jitter.Dynamics;
using LifeFall.Logic.Sequences;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Logic
{
    public class GameObjectManager : DrawableGameComponent
    {
        SortedList<int, Sequence> sequences = new SortedList<int, Sequence>();
        public Dictionary<RigidBody, MovableObject> Obstacles;

#region Methods
        public GameObjectManager(Game game)
            : base(game)
        {
            
        }

#endregion Methods
    }
}
