using FantaRPG.src.Interfaces;
using FantaRPG.src.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class Player : IEntity
    {
        private Dictionary<string, Keys> Input;
        public IShapeF Bounds { get; }
        private Texture2D Texture;
        public Vector2 Velocity;
        private Vector2 Acceleration;
        int spellSize = 10;
        bool onGround = false;
        bool canJump = true;
        int jumpCount = 1;
        int jumpCountMax = 1;
        bool onWall = false;
        private Stats Stats = new Stats();
        public Player(Texture2D texture, Dictionary<string, Keys> input, IShapeF shape)
        {
            Input = input;
            Bounds = shape;
        }
        public void Update(GameTime gameTime)
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
            Vector2 actualMovementVector = Vector2.Zero;
            if (movementVector != Vector2.Zero)
            {
                actualMovementVector = Vector2.Normalize(movementVector);
                actualMovementVector.X *= Stats.MoveSpeed * 1000f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (MovementInput.KeyJustDown(Input["Jump"]))
            {
                if (onWall && canJump)
                {
                    Velocity.X = (-movementVector.X) * 500f;
                    Velocity.Y = -1000;
                    canJump = false;
                }
                else
                {
                    if (jumpCount > 0)
                    {
                        jumpCount--;
                        Velocity.Y = -1000;
                        onGround = false;
                    }
                }
            }
            /*if (MovementInput.MouseLeftJustDown())
            {
                Vector2 playerCenter = new Vector2(Position.X + HitboxSize.X / 2, Position.Y + HitboxSize.Y / 2);
                Vector2 cursorPos = new Vector2(Mouse.GetState().Position.X - Game1.Instance.cam.Transform.Translation.X, Mouse.GetState().Position.Y - Game1.Instance.cam.Transform.Translation.Y);

                //Debug.WriteLine(cursorPos.X.ToString("0.0") + ";" + cursorPos.Y.ToString("0.0") + " cursorpos, " + Position.X.ToString("0.0") + ";" + Position.Y.ToString("0.0") + " playerpos");
                Vector2 spellVel = new Vector2(cursorPos.X - playerCenter.X, cursorPos.Y - playerCenter.Y);
                spellVel.Normalize();
                spellVel = Vector2.Multiply(spellVel, 5000);
                Game1.Instance.CurrentRoom.AddEntity(new Bullet(Game1.Instance.pixel, (int)(playerCenter.X - spellSize / 2), (int)(playerCenter.Y - spellSize / 2), spellSize, spellSize, spellVel));
            }*/
            Acceleration += actualMovementVector;
            Velocity.X += Acceleration.X;
            Velocity.X = Math.Clamp(Velocity.X, -Stats.MoveSpeed * 500, Stats.MoveSpeed * 500);
            Velocity.Y += Acceleration.Y;
            /*bool tempOnWall = false;
            foreach (var item in Game1.Instance.CurrentRoom.Platforms.Where(x => x.IsCollidable))
            {
                if (item.IsDoor)
                {
                    if (IsTouchingLeft(item, gameTime) ||
                        IsTouchingRight(item, gameTime) ||
                        IsTouchingTop(item, gameTime) ||
                        IsTouchingBottom(item, gameTime))
                    {
                        Game1.Instance.ChangeRoom(item.targetRoom);
                    }
                }
                else
                {
                    if (IsTouchingLeft(item, gameTime))
                    {
                        Position.X = item.Position.X - HitboxSize.X;
                        Velocity.X = 0;
                        tempOnWall = true;
                        canJump = true;
                    }
                    else if (IsTouchingRight(item, gameTime))
                    {
                        Position.X = item.Position.X + item.HitboxSize.X;
                        Velocity.X = 0;
                        tempOnWall = true;
                        canJump = true;
                    }
                    if (IsTouchingTop(item, gameTime))
                    {
                        Position.Y = item.Position.Y - HitboxSize.Y;
                        Velocity.Y = 0;
                        onGround = true;
                        canJump = true;
                        jumpCount = jumpCountMax;
                    }
                    else if (IsTouchingBottom(item, gameTime))
                    {
                        Position.Y = item.Position.Y + item.HitboxSize.Y;
                        Velocity.Y = 0;
                    }
                }
            }*/
            if (Math.Sign(actualMovementVector.X) == 0 && onGround)
            {
                float drag = 1000f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Velocity.X - drag >= 0)
                {
                    Velocity.X -= drag;
                }
                else if (Velocity.X + drag <= 0)
                {
                    Velocity.X += drag;
                }
                else
                {
                    Velocity.X = 0;
                }
            }
            Bounds.Position += Vector2.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (Math.Abs(Velocity.X) < 0.001)
            {
                Velocity.X = 0;
            }
            if (Math.Abs(Velocity.Y) < 0.001)
            {
                Velocity.Y = 0;
            }
            Acceleration = Vector2.Zero;
            Debug.WriteLine(Bounds.Position.ToString());
            //onWall = tempOnWall;
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            Bounds.Position -= collisionInfo.PenetrationVector;
            Velocity.Y = 0;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.Instance.debugFont, "onWall: " + onWall + "\nonGround: " + onGround + "\njumps remaining: " + jumpCount, Bounds.Position + new Vector2(0, 20), Color.Red);
        }
    }
}
