using Microsoft.Xna.Framework;

namespace FantaRPG.src
{
    internal class BasicCollision
    {
        protected Vector2 position, velocity, hitboxSize;
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public Vector2 HitboxSize { get { return hitboxSize; } set { hitboxSize = value; } }
        public Vector2 Center { get { return position + (hitboxSize / 2); } set { position.X = value.X - hitboxSize.X / 2; position.Y = value.Y - hitboxSize.Y / 2; } }
        public bool IsTouching(BasicCollision sprite, GameTime gameTime)
        {
            return IsTouchingLeft(sprite, gameTime) || IsTouchingRight(sprite, gameTime) || IsTouchingBottom(sprite, gameTime) || IsTouchingTop(sprite, gameTime);
        }
        public bool IsTouchingLeft(BasicCollision sprite, GameTime gameTime)
        {
            return Position.X + HitboxSize.X + Velocity.X * gameTime.ElapsedGameTime.TotalSeconds > sprite.Position.X &&
                   Position.X < sprite.Position.X &&
                   Position.Y + HitboxSize.Y > sprite.Position.Y &&
                   Position.Y < sprite.Position.Y + sprite.HitboxSize.Y;
        }

        public bool IsTouchingRight(BasicCollision sprite, GameTime gameTime)
        {
            return Position.X + Velocity.X * gameTime.ElapsedGameTime.TotalSeconds < sprite.Position.X + sprite.HitboxSize.X &&
                   Position.X + HitboxSize.X > sprite.Position.X + sprite.HitboxSize.X &&
                   Position.Y + HitboxSize.Y > sprite.Position.Y &&
                   Position.Y < sprite.Position.Y + sprite.HitboxSize.Y;
        }

        public bool IsTouchingTop(BasicCollision sprite, GameTime gameTime)
        {
            return Position.Y + HitboxSize.Y + Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds > sprite.Position.Y &&
                   Position.Y < sprite.Position.Y &&
                   Position.X + HitboxSize.X > sprite.Position.X &&
                   Position.X < sprite.Position.X + sprite.HitboxSize.X;
        }

        public bool IsTouchingBottom(BasicCollision sprite, GameTime gameTime)
        {
            return Position.Y + Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds < sprite.Position.Y + sprite.HitboxSize.Y &&
                   Position.Y + HitboxSize.Y > sprite.Position.Y + sprite.HitboxSize.Y &&
                   Position.X + HitboxSize.X > sprite.Position.X &&
                   Position.X < sprite.Position.X + sprite.HitboxSize.X;
        }
    }
}