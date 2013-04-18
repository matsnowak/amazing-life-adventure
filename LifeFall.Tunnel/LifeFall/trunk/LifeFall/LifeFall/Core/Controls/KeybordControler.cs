using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace LifeFall.Core.Controls
{
    class KeyboardControler : IControler
    {
        KeyboardState keyboardState;
        bool isLeftDown = false;
        bool isRightDown = false;

        public KeyboardControler()
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
            if ((keyboardState.IsKeyDown(Keys.RightAlt) && !isRightDown ))
            {
                isRightDown = true;
                return true;
            }

            if ( keyboardState.IsKeyDown(Keys.LeftAlt) && !isLeftDown)
            {
                isLeftDown = true;
                return true;
            }

            if (keyboardState.IsKeyUp(Keys.LeftAlt))
            {
                isLeftDown = false;               
            }
            if (keyboardState.IsKeyUp(Keys.RightAlt))
            {
                isRightDown = false;
            }
            return false;
           
        }

        public bool Pause()
        {
            return keyboardState.IsKeyDown(keyPause);
        }

        public bool Accept()
        {
            return keyboardState.IsKeyDown(keyAccept);
        }

        //TODO: Zmienic bo tak nie może być!!!
        public void LoadSettings(Keys left, Keys right, Keys up, Keys down)
        {
            keyLeft = left;
            keyRight = right;
            keyUp = up;
            keyDown = down;
        }

        public void LoadSettings()
        {

        }

        public void Update()
        {
            keyboardState = Keyboard.GetState();
        }
    }
}
