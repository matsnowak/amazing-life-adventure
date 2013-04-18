using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFall.Core.Trigger_System
{
    public class Trigger
    {
        public delegate void action();
        public delegate bool condition();

        public action Action { get; set; }
        public condition Condition { get; set; }

        public bool TriggerOnce { get; set; }
        public bool Enabled { get; set; }

        public Trigger()
        {
            Enabled = true;
            TriggerOnce = true;
        }
        
        public virtual void CheckTrigger()
        {
            if (Condition())
            {
                if (TriggerOnce)
                    Enabled = false;
                Action();
                
            }

            
        }
    }
}
