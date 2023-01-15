using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG
{
    internal class Player : Entity
    {
        private Dictionary<string, Keys> Input;
        private Vector2 Acceleration;
        int spellSize = 10;

        public Player(Texture2D texture, Dictionary<string, Keys> input) : base(texture)
        {
            Input = input;
            Velocity = new Vector2();
            Position = new Vector2(-400, -400);
        }
        public new void Update(GameTime gameTime)
        {
            Vector2 movementVector = Vector2.Zero;
            Acceleration.Y += 2000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (MovementInput.KeyDown(Input["Up"]))
            {
                movementVector -= Vector2.UnitY;
            }
            if (MovementInput.KeyDown(Input["Down"]))
            {
                movementVector += Vector2.UnitY;
            }
            if (MovementInput.KeyDown(Input["Left"]))
            {
                movementVector -= Vector2.UnitX;
            }
            if (MovementInput.KeyDown(Input["Right"]))
            {
                movementVector += Vector2.UnitX;
            }
            if (movementVector != Vector2.Zero)
            {
                movementVector.Normalize();
                movementVector.X *= 500;
                movementVector.X *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
            if (MovementInput.KeyJustDown(Input["Jump"]))
            {
                Velocity.Y = -1000;
            }
            if (MovementInput.MouseLeftJustDown())
            {
                Vector2 playerCenter = new Vector2(Position.X + (HitboxSize.X / 2), Position.Y + (HitboxSize.Y / 2));
                Vector2 cursorPos = new Vector2(Mouse.GetState().Position.X - Game1.Instance.cam.Transform.Translation.X, Mouse.GetState().Position.Y - Game1.Instance.cam.Transform.Translation.Y);

                
                Debug.WriteLine(cursorPos.X.ToString("0.0") + ";" + cursorPos.Y.ToString("0.0") + " cursorpos, " + Position.X.ToString("0.0") + ";" + Position.Y.ToString("0.0") + " playerpos");
                Vector2 spellVel = new Vector2(cursorPos.X - playerCenter.X, cursorPos.Y - playerCenter.Y);
                spellVel.Normalize();
                spellVel = Vector2.Multiply(spellVel, 1000);
                Game1.Instance.CurrentRoom.AddEntity(new Spell(Game1.Instance.pixel, (int)(playerCenter.X-(spellSize/2)), (int)(playerCenter.Y-(spellSize/2)), spellSize, spellSize, spellVel));
            }

            Acceleration += movementVector;
            Velocity += Acceleration;
            foreach (var item in Game1.Instance.CurrentRoom.Platforms)
            {
                if (IsTouchingLeft(item, gameTime))
                {
                    Position.X = item.Position.X - HitboxSize.X;
                    Velocity.X = 0;
                }
                else if (IsTouchingRight(item, gameTime))
                {
                    Position.X = item.Position.X + item.HitboxSize.X;
                    Velocity.X = 0;
                }
                if (IsTouchingTop(item, gameTime))
                {
                    Position.Y = item.Position.Y - HitboxSize.Y;
                    Velocity.Y = 0;
                }
                else if (IsTouchingBottom(item, gameTime))
                {
                    Position.Y = item.Position.Y + item.HitboxSize.Y;
                    Velocity.Y = 0;
                }
            }
            Position += (Vector2.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds));
            Velocity *= 0.99f;
            if (Math.Abs(Velocity.X) < 0.001)
            {
                Velocity.X = 0;
            }
            if (Math.Abs(Velocity.Y) < 0.001)
            {
                Velocity.Y = 0;
            }
            Acceleration = Vector2.Zero;
        }
    }
}
