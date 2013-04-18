using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace LifeFallPrototype.Core.Controls
{
    class KeyboardControler : IControler
    {
        KeyboardState keyboardState;

        KeyboardControler()
        {
            keyboardState = Keyboard.GetState();
        }

        #region Definitions

        private Keys keyLeft;
        private Keys keyRight;
        private Keys keyUp;
        private Keys keyDown;

        private Keys keyAction;
        private Keys keyPause;
        private Keys keyAccept;

        #endregion Definitions

        public bool Left()
        {
            return keyboardState.IsKeyDown(keyLeft);           
        }

        public bool Right()
        {
            return keyboardState.IsKeyDown(keyRight);
        }

        public bool Up()
        {
            return keyboardState.IsKeyDown(keyUp);
        }

        public bool Down()
        {
            return keyboardState.IsKeyDown(keyDown);
        }

        public bool Action()
        {
            return keyboardState.IsKeyDown(keyAction);
        }

        public bool Pause()
        {
            return keyboardState.IsKeyDown(keyPause);
        }

        public bool Accept()
        {
            return keyboardState.IsKeyDown(keyAccept);
        }

        public void LoadSettings()
        {
            throw new NotImplementedException();
        }
    }
}
