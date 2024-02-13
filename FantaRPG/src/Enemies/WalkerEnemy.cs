using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace FantaRPG.src.Enemies
{
    internal class WalkerEnemy : Entity
    {
        private Entity target;

        public Entity Target
        {
            protected get { return target; }
            set { target = value; }
        }

        public WalkerEnemy(float x, float y, Vector2 size, Texture2D texture = null) : base(x, y, size, texture)
        {
            gravityAffected = true;
            collisionAffected = true;
            Stats[Stat.MoveSpeed] = 40;
        }
        public override void Update(GameTime gameTime)
        {
            Vector2 movementVector = Vector2.Zero;

            // Handle gravity
            if (gravityAffected)
            {
                acceleration.Y += Game1.Instance.CurrentRoom.Gravity * 2000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Determine movement direction based on the player's position
            if (Target.Center.X < Center.X)
            {
                movementVector.X = -1; // Move left
            }
            else if (Target.Center.X > Center.X)
            {
                movementVector.X = 1; // Move right
            }

            // Normalize and apply movement speed to the horizontal movement vector
            Vector2 actualMovementVector = Vector2.Zero;
            if (movementVector != Vector2.Zero)
            {
                actualMovementVector = Vector2.Normalize(movementVector);
                actualMovementVector.X *= Stats[Stat.MoveSpeed] * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Add the movement vector to acceleration
            acceleration += actualMovementVector;

            // Update velocity with acceleration, respecting maximum move speed and direction change
            if (Math.Abs(velocity.X + acceleration.X) < Stats[Stat.MoveSpeed] * 10 || Math.Sign(acceleration.X) != Math.Sign(velocity.X))
            {
                velocity.X += acceleration.X;
            }
            velocity.Y += acceleration.Y;

            // Reset acceleration for the next frame
            acceleration = Vector2.Zero;

            // Handle collisions with platforms
            foreach (Platform platform in Game1.Instance.CurrentRoom.Platforms)
            {
                HandlePlatformCollisions(platform, gameTime);
            }

            // Apply drag if not moving and on the ground
            if (Math.Sign(actualMovementVector.X) == 0)
            {
                ApplyDrag(gameTime);
            }

            // Update position based on velocity
            Position += Vector2.Multiply(velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);

            // Reset small velocities to zero to prevent drifting
            if (Math.Abs(velocity.X) < 0.001) velocity.X = 0;
            if (Math.Abs(velocity.Y) < 0.001) velocity.Y = 0;
        }

        private void ApplyDrag(GameTime gameTime)
        {
            float drag = 50 * Stats[Stat.MoveSpeed] * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Math.Abs(velocity.X) > drag)
            {
                velocity.X -= Math.Sign(velocity.X) * drag;
            }
            else
            {
                velocity.X = 0;
            }
        }

        private void HandlePlatformCollisions(Platform platform, GameTime gameTime)
        {
            if (!platform.IsCollidable) return;

            // Platform top collision
            if (IsTouchingTopOf(platform, gameTime))
            {
                position.Y = platform.Position.Y - HitboxSize.Y;
                velocity.Y = 0;
            }

            // Platform bottom collision
            if (IsTouchingBottomOf(platform, gameTime))
            {
                position.Y = platform.Position.Y + platform.HitboxSize.Y;
                velocity.Y = 0;
            }

            // Platform side collisions
            if (IsTouchingLeftOf(platform, gameTime))
            {
                position.X = platform.Position.X - HitboxSize.X;
                velocity.X = 0;
            }
            else if (IsTouchingRightOf(platform, gameTime))
            {
                position.X = platform.Position.X + platform.HitboxSize.X;
                velocity.X = 0;
            }
        }
    }
}
