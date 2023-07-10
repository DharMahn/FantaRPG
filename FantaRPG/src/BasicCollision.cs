using Microsoft.Xna.Framework;

namespace FantaRPG.src
{
    internal class BasicCollision
    {
        protected Vector2 position, velocity, hitboxSize;
        public Vector2 Position { get { return position; } protected set { position = value; } }
        public Vector2 Velocity { get { return velocity; } protected set { velocity = value; } }
        public Vector2 HitboxSize { get { return hitboxSize; } protected set { hitboxSize = value; } }

        public bool IsTouchingLeft(Entity sprite, GameTime gameTime)
        {
            return Position.X + HitboxSize.X + Velocity.X * gameTime.ElapsedGameTime.TotalSeconds > sprite.Position.X &&
                   Position.X < sprite.Position.X &&
                   Position.Y + HitboxSize.Y > sprite.Position.Y &&
                   Position.Y < sprite.Position.Y + sprite.HitboxSize.Y;
        }

        public bool IsTouchingRight(Entity sprite, GameTime gameTime)
        {
            return Position.X + Velocity.X * gameTime.ElapsedGameTime.TotalSeconds < sprite.Position.X + sprite.HitboxSize.X &&
                   Position.X + HitboxSize.X > sprite.Position.X + sprite.HitboxSize.X &&
                   Position.Y + HitboxSize.Y > sprite.Position.Y &&
                   Position.Y < sprite.Position.Y + sprite.HitboxSize.Y;
        }

        public bool IsTouchingTop(Entity sprite, GameTime gameTime)
        {
            return Position.Y + HitboxSize.Y + Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds > sprite.Position.Y &&
                   Position.Y < sprite.Position.Y &&
                   Position.X + HitboxSize.X > sprite.Position.X &&
                   Position.X < sprite.Position.X + sprite.HitboxSize.X;
        }

        public bool IsTouchingBottom(Entity sprite, GameTime gameTime)
        {
            return Position.Y + Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds < sprite.Position.Y + sprite.HitboxSize.Y &&
                   Position.Y + HitboxSize.Y > sprite.Position.Y + sprite.HitboxSize.Y &&
                   Position.X + HitboxSize.X > sprite.Position.X &&
                   Position.X < sprite.Position.X + sprite.HitboxSize.X;
        }
    }
}