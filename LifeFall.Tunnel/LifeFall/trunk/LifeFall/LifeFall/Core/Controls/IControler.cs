using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace LifeFall.Core.Controls
{
    public interface IControler
    {
        Boolean Left();
        Boolean Right();
        Boolean Up();
        Boolean Down();

        Boolean Action();
        Boolean Pause();
        Boolean Accept();

        void LoadSettings();
        void LoadSettings(Keys left, Keys right, Keys up, Keys down);

        void Update();

    }
}
