using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG
{
    internal class Player : Entity
    {
        private Dictionary<string, Keys> Input;
        private Vector2 Velocity;
        private Vector2 Acceleration;

        public Player(Texture2D texture, Dictionary<string, Keys> input) : base(texture)
        {
            Input = input;
        }
        public void Update(GameTime gameTime)
        {
            Vector2 movementVector = Vector2.Zero;
            Acceleration += Vector2.UnitY;
            if (MovementInput.KeyDown(Input["Up"]))
            {
                movementVector -= Vector2.UnitY * 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (MovementInput.KeyDown(Input["Down"]))
            {
                movementVector += Vector2.UnitY * 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (MovementInput.KeyDown(Input["Left"]))
            {
                movementVector -= Vector2.UnitX * 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (MovementInput.KeyDown(Input["Right"]))
            {
                movementVector += Vector2.UnitX * 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (movementVector != Vector2.Zero)
            {
                movementVector.Normalize();
            }
            if (MovementInput.KeyJustDown(Input["Jump"]))
            {
                Velocity.Y = -30;
            }
            Acceleration += movementVector;
            Velocity += Acceleration;
            Position += Velocity;

            if (Position.Y > 0)
            {
                Position.Y = 0;
            }
            Velocity *= 0.95f;
            if (Math.Abs(Velocity.X) < 0.0001)
            {
                Velocity.X = 0;
            }
            if (Math.Abs(Velocity.Y) < 0.0001)
            {
                Velocity.Y = 0;
            }
            Acceleration = Vector2.Zero;
        }
    }
}
