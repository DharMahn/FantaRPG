using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Modifiers
{
    internal class CurvedVelocityBehavior : IBulletBehavior
    {
        private float rotationSpeed; // Degrees per second
        public int PassCount { get; set; } = 0;

        // Constructor to set rotation speed
        public CurvedVelocityBehavior(float rotationSpeed)
        {
            this.rotationSpeed = rotationSpeed;
        }

        public void ActOnCollision(object sender, EventArgs e)
        {
            // Collision logic here, if needed
        }

        public void Update(Bullet bullet, GameTime gameTime)
        {
            // Calculate the amount to rotate this frame
            float rotationAmount = rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Rotate the velocity vector
            bullet.Velocity = RotateVector(bullet.Velocity, rotationAmount);
        }

        public void Execute(Bullet bullet)
        {
            // Immediate execution logic here, if needed
        }

        public IBulletBehavior Clone()
        {
            var toreturn = new CurvedVelocityBehavior(rotationSpeed);
            return toreturn;
        }

        // Utility method to rotate a vector by a given amount in degrees
        private Vector2 RotateVector(Vector2 vector, float degrees)
        {
            float radians = MathHelper.ToRadians(degrees);
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);
            return new Vector2(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos
            );
        }
    }
}
