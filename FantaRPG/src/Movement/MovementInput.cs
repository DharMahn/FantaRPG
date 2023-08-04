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
            return Game1.Instance.IsActive && Keyboard.GetState().IsKeyDown(key);
        }
        public static bool KeyUp(Keys key)
        {
            return Game1.Instance.IsActive && Keyboard.GetState().IsKeyUp(key);
        }
        public static bool KeyJustDown(Keys key)
        {
            return Game1.Instance.IsActive && Keyboard.GetState().IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }
        public static bool KeyJustUp(Keys key)
        {
            return Game1.Instance.IsActive && Keyboard.GetState().IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
        }
        public static bool MouseLeftDown()
        {
            return Game1.Instance.IsActive && Mouse.GetState().LeftButton == ButtonState.Pressed;
        }
        public static bool MouseLeftUp()
        {
            return Game1.Instance.IsActive && Mouse.GetState().LeftButton == ButtonState.Released;
        }
        public static bool MouseLeftJustDown()
        {
            return Game1.Instance.IsActive && Mouse.GetState().LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
        }
        public static bool MouseLeftJustUp()
        {
            return Game1.Instance.IsActive && Mouse.GetState().LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed;
        }
        public static Point MousePosition()
        {
            return Mouse.GetState().Position;
        }
        public static bool MouseRightDown()
        {
            return Game1.Instance.IsActive && Mouse.GetState().RightButton == ButtonState.Pressed;
        }
        public static bool MouseRightUp()
        {
            return Game1.Instance.IsActive && Mouse.GetState().RightButton == ButtonState.Released;
        }
        public static bool MouseRightJustDown()
        {
            return Game1.Instance.IsActive && Mouse.GetState().RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released;
        }
        public static bool MouseRightJustUp()
        {
            return Game1.Instance.IsActive && Mouse.GetState().RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed;
        }
        public static void Update()
        {
            lastKeyboardState = Keyboard.GetState();
            lastMouseState = Mouse.GetState();
        }
    }
}