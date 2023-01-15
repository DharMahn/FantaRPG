using Microsoft.Xna.Framework;

namespace FantaRPG
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }

        internal void Follow(Entity target)
        {
            var position = Matrix.CreateTranslation(-target.Position.X - (target.HitboxSize.X / 2), -target.Position.Y - (target.HitboxSize.Y / 2), 0);
            var offset = Matrix.CreateTranslation(Game1.Instance._graphics.PreferredBackBufferWidth / 2, Game1.Instance._graphics.PreferredBackBufferHeight - (Game1.Instance._graphics.PreferredBackBufferHeight / 2.5f), 0);
            Transform = position * offset;
        }
    }
}