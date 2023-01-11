using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace FantaRPG
{
    public static class MovementInput
    {
        private static KeyboardState lastKeyboardState;
        public static bool KeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
        public static bool KeyUp(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key);
        }
        public static bool KeyJustDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }
        public static bool KeyJustUp(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
        }
        public static void Update()
        {
            lastKeyboardState = Keyboard.GetState();
        }
    }
}