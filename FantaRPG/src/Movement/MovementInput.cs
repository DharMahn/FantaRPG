using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FantaRPG.src.Movement
{
    public static class MovementInput
    {
        private static KeyboardState lastKeyboardState;
        private static MouseState lastMouseState;
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
        public static bool MouseLeftDown()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }
        public static bool MouseLeftUp()
        {
            return Mouse.GetState().LeftButton == ButtonState.Released;
        }
        public static bool MouseLeftJustDown()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
        }
        public static bool MouseLeftJustUp()
        {
            return Mouse.GetState().LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed;
        }
        public static Point MousePosition()
        {
            return Mouse.GetState().Position;
        }
        public static bool MouseRightDown()
        {
            return Mouse.GetState().RightButton == ButtonState.Pressed;
        }
        public static bool MouseRightUp()
        {
            return Mouse.GetState().RightButton == ButtonState.Released;
        }
        public static bool MouseRightJustDown()
        {
            return Mouse.GetState().RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released;
        }
        public static bool MouseRightJustUp()
        {
            return Mouse.GetState().RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed;
        }
        public static void Update()
        {
            lastKeyboardState = Keyboard.GetState();
            lastMouseState = Mouse.GetState();
        }
    }
}