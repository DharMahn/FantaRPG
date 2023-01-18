using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace FantaRPG.src
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }

        internal void Follow(Entity target)
        {
            float targetX, targetY, offsetX, offsetY;
            targetX = -target.Position.X - target.HitboxSize.X / 2f;
            targetY = -target.Position.Y - target.HitboxSize.Y / 2f + Game1.Instance._graphics.PreferredBackBufferHeight * 0.2f;
            offsetX = Game1.Instance._graphics.PreferredBackBufferWidth / 2f;
            offsetY = Game1.Instance._graphics.PreferredBackBufferHeight - Game1.Instance._graphics.PreferredBackBufferHeight / 2.5f;
            if (targetX < Game1.Instance.CurrentRoom.Bounds.X)
            {
                targetX = Game1.Instance.CurrentRoom.Bounds.X;
            }
            if (targetY < Game1.Instance.CurrentRoom.Bounds.Y)
            {
                targetY = Game1.Instance.CurrentRoom.Bounds.Y;
            }
            if (targetX > Game1.Instance.CurrentRoom.Bounds.Width - offsetX)
            {
                targetX = Game1.Instance.CurrentRoom.Bounds.Width - offsetX;
            }
            if (targetY > Game1.Instance.CurrentRoom.Bounds.Height - offsetY)
            {
                targetY = Game1.Instance.CurrentRoom.Bounds.Height - offsetY;
            }
            var position = Matrix.CreateTranslation(
                targetX,
                targetY, 0);
            var offset = Matrix.CreateTranslation(offsetX, offsetY, 0);
            Transform = position * offset;
            //Debug.WriteLine("X: " + targetX + " Y: " + targetY);
        }
    }
}