using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LifeFallPrototype.Core.Controls
{
    interface IControler
    {
        Boolean Left();
        Boolean Right();
        Boolean Up();
        Boolean Down();

        Boolean Action();
        Boolean Pause();
        Boolean Accept();

        void LoadSettings();

    }
}
