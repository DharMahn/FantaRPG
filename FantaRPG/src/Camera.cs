using FantaRPG.src.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
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
            set { bounds = value; }
        }

        internal void Follow(IEntity target)
        {
            float targetX, targetY, offsetX, offsetY;
            RectangleF targetRect = (RectangleF)target.Bounds;
            targetX = -targetRect.X - targetRect.Width / 2f;
            targetY = -targetRect.Y - targetRect.Height / 2f + Game1.Instance._graphics.PreferredBackBufferHeight * 0.2f;
            offsetX = Game1.Instance._graphics.PreferredBackBufferWidth / 2f;
            offsetY = Game1.Instance._graphics.PreferredBackBufferHeight - Game1.Instance._graphics.PreferredBackBufferHeight / 2.5f;
            bounds.X = (int)(targetX - offsetX);
            bounds.Y = (int)(targetY - offsetY);
            bounds.Width = Game1.Instance._graphics.PreferredBackBufferWidth;
            bounds.Height = Game1.Instance._graphics.PreferredBackBufferHeight;
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