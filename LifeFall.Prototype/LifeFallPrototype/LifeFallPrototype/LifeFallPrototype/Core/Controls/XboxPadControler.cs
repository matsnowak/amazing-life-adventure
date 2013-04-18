﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LifeFallPrototype.Core.Controls
{
    class XboxPadControler : IControler
    {
        GamePadState gamePadState;

        XboxPadControler()
        {
            gamePadState = GamePad.GetState(playerIndex);
        }

        #region Definitions

        private PlayerIndex playerIndex;

        #endregion Definitions


        public bool Left()
        {
            return gamePadState.DPad.Left == ButtonState.Pressed;
        }

        public bool Right()
        {
            return gamePadState.DPad.Right == ButtonState.Pressed;
        }

        public bool Up()
        {
            return gamePadState.DPad.Up == ButtonState.Pressed ;
        }

        public bool Down()
        {
            return gamePadState.DPad.Down == ButtonState.Pressed;
        }

        public bool Action()
        {
            return gamePadState.Buttons.A == ButtonState.Pressed;
        }

        public bool Pause()
        {
            return gamePadState.Buttons.Back == ButtonState.Pressed;
        }

        public bool Accept()
        {
            return gamePadState.Buttons.B == ButtonState.Pressed;
        }

        public void LoadSettings()
        {
            throw new NotImplementedException();
        }
    }
}