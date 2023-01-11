using Microsoft.Xna.Framework;

namespace FantaRPG
{
    public class Camera
    {
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Center => new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2));
        public float Left => Position.X;
        public float Top => Position.Y;
        public float Right => Position.X + Size.X;
        public float Bottom => Position.Y + Size.Y;
        public Camera(float x, float y, float w, float h)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(w, h);
        }
        public void SetCenter(float x, float y)
        {
            Position.X = x - (Size.X / 2);
            Position.Y = y - (Size.Y / 2);
        }
    }
}