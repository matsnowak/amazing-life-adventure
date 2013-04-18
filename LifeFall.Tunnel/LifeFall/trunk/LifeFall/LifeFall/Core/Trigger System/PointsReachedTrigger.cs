using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Core.Trigger_System
{
    public class PointsReachedTrigger: Trigger
    {
        Player p;
        int minScore;
        public PointsReachedTrigger(Player player, int minScore):base()
        {

            Condition = delegate()
            {
                return p.score >= minScore;
            };
        }
    }
}
