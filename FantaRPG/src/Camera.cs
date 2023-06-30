using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace FantaRPG.src
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }
        private Rectangle bounds;

        public Rectangle Bounds
        {
            get { return bounds; }
            private set { bounds = value; }
        }

        public Camera()
        {
            bounds = new Rectangle();
        }

        internal void Follow(Entity target)
        {
            float targetX, targetY, offsetX, offsetY;
            targetX = -target.Bounds.Position.X - target.HitboxSize.X / 2f;
            targetY = -target.Bounds.Position.Y - target.HitboxSize.Y / 2f + Game1.Instance._graphics.PreferredBackBufferHeight * 0.2f;
            offsetX = Game1.Instance._graphics.PreferredBackBufferWidth / 2f;
            offsetY = Game1.Instance._graphics.PreferredBackBufferHeight - Game1.Instance._graphics.PreferredBackBufferHeight / 2.5f;
            bounds.X = (int)(target.Bounds.Position.X - offsetX);
            bounds.Y = (int)(target.Bounds.Position.Y - offsetY);
            bounds.Width = Game1.Instance._graphics.PreferredBackBufferWidth;
            bounds.Height = Game1.Instance._graphics.PreferredBackBufferHeight;
            targetX = MathHelper.Clamp(
                targetX, 
                -(Game1.Instance.CurrentRoom.Bounds.X - (Game1.Instance._graphics.PreferredBackBufferWidth / 2)), 
                -Game1.Instance._graphics.PreferredBackBufferWidth  / 2);

            var position = Matrix.CreateTranslation(
                targetX,
                targetY, 0);
            var offset = Matrix.CreateTranslation(offsetX, offsetY, 0);
            Transform = position * offset;
        }
    }
}