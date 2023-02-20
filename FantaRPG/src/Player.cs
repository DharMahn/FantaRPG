using FantaRPG.src.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class Player : Entity
    {
        private Dictionary<string, Keys> Input;
        private Vector2 Acceleration;
        int spellSize = 10;
        bool onGround = false;
        bool canJump = true;
        int jumpCount = 1;
        int jumpCountMax = 1;
        bool onWall = false;
        bool onLeftWall;
        bool onRightWall;
        public Player(Texture2D texture, Dictionary<string, Keys> input, int x, int y, int w, int h) : base(texture, x, y, w, h)
        {
            Input = input;
        }
        public override void Update(GameTime gameTime)
        {
            Vector2 movementVector = Vector2.Zero;
            Acceleration.Y += 2000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (MovementInput.KeyDown(Input["Up"]))
            {
                //movementVector -= Vector2.UnitY;
            }
            if (MovementInput.KeyDown(Input["Down"]))
            {
                //movementVector += Vector2.UnitY;
            }
            if (MovementInput.KeyDown(Input["Left"]))
            {
                movementVector -= Vector2.UnitX;
                if (onWall && onLeftWall)
                {
                    onWall = false;
                }
            }
            if (MovementInput.KeyDown(Input["Right"]))
            {
                movementVector += Vector2.UnitX;
                if (onWall && onRightWall)
                {
                    onWall = false;
                }
            }
            Vector2 actualMovementVector = Vector2.Zero;
            if (movementVector != Vector2.Zero)
            {
                actualMovementVector = Vector2.Normalize(movementVector);
                actualMovementVector.X *= Stats.GetStat(Stat.MoveSpeed) * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (MovementInput.KeyJustDown(Input["Jump"]))
            {
                if (onWall && canJump)
                {
                    if (onLeftWall)
                    {
                        velocity.X = -500f; 
                        velocity.Y = -1250;
                        onLeftWall = false;
                        onWall = false;
                    }
                    else if (onRightWall)
                    {
                        velocity.X = 500f; 
                        velocity.Y = -1250;
                        onRightWall = false;
                        onWall = false; 
                    }
                    else
                    {
                        velocity.Y = -50 * Stats.GetStat(Stat.JumpStrength);
                    }
                    canJump = false;
                }
                else
                {
                    if (jumpCount > 0)
                    {
                        jumpCount--;
                        velocity.Y = -50 * Stats.GetStat(Stat.JumpStrength);
                        onGround = false;
                    }
                }
            }
            if (MovementInput.MouseLeftJustDown())
            {
                Vector2 playerCenter = new Vector2(Position.X + HitboxSize.X / 2, Position.Y + HitboxSize.Y / 2);
                Vector2 cursorPos = new Vector2(Mouse.GetState().Position.X - Game1.Instance.cam.Transform.Translation.X, Mouse.GetState().Position.Y - Game1.Instance.cam.Transform.Translation.Y);

                //Debug.WriteLine(cursorPos.X.ToString("0.0") + ";" + cursorPos.Y.ToString("0.0") + " cursorpos, " + Position.X.ToString("0.0") + ";" + Position.Y.ToString("0.0") + " playerpos");
                Vector2 spellVel = new Vector2(cursorPos.X - playerCenter.X, cursorPos.Y - playerCenter.Y);
                spellVel.Normalize();
                spellVel = Vector2.Multiply(spellVel, 1000);
                Game1.Instance.CurrentRoom.AddEntity(new Bullet(Game1.Instance.pixel, (int)(playerCenter.X - spellSize / 2), (int)(playerCenter.Y - spellSize / 2), spellSize, spellSize, spellVel));
            }
            if (/*onGround*/true)
            {
                Acceleration += actualMovementVector;
                velocity.X += Acceleration.X;
                //velocity.X = Math.Sign(velocity.X) == Math.Sign(Acceleration.X) ? Velocity.X + Acceleration.X : (Velocity.X / 2) + Acceleration.X;
                velocity.X = Math.Clamp(velocity.X, -Stats.GetStat(Stat.MoveSpeed) * 10, Stats.GetStat(Stat.MoveSpeed) * 10);
                velocity.Y += Acceleration.Y;
            }
            onGround = false;
            foreach (var item in Game1.Instance.CurrentRoom.Objects)
            {
                if (item.IsCollidable)
                {
                    if (IsTouchingTop(item, gameTime))
                    {
                        position.Y = item.Position.Y - HitboxSize.Y;
                        velocity.Y = 0;
                        onGround = true;
                        canJump = true;
                        jumpCount = jumpCountMax;
                        onLeftWall = false;
                        onRightWall = false;
                    }
                    else if (IsTouchingBottom(item, gameTime))
                    {
                        position.Y = item.Position.Y + item.HitboxSize.Y;
                        velocity.Y = 0;
                    }
                    if (IsTouchingLeft(item, gameTime))
                    {
                        position.X = item.Position.X - HitboxSize.X;
                        velocity.X = 0;
                        onWall = true;
                        onLeftWall = true;
                        onRightWall = false;
                        canJump = true;
                    }
                    else if (IsTouchingRight(item, gameTime))
                    {
                        position.X = item.Position.X + item.HitboxSize.X;
                        velocity.X = 0;
                        onWall = true;
                        onRightWall = true;
                        onLeftWall = false;
                        canJump = true;
                    }
                }
                else
                {
                    if (item is Portal)
                    {
                        if (IsTouchingLeft(item, gameTime) ||
                            IsTouchingRight(item, gameTime) ||
                            IsTouchingTop(item, gameTime) ||
                            IsTouchingBottom(item, gameTime))
                        {
                            ((Portal)item).ChangeRoom();
                        }
                    }
                }
            }
            if (Math.Sign(actualMovementVector.X) == 0 && onGround)
            {
                float drag = 1000f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Velocity.X - drag >= 0)
                {
                    velocity.X -= drag;
                }
                else if (Velocity.X + drag <= 0)
                {
                    velocity.X += drag;
                }
                else
                {
                    velocity.X = 0;
                }
            }
            Position += Vector2.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (Math.Abs(Velocity.X) < 0.001)
            {
                velocity.X = 0;
            }
            if (Math.Abs(Velocity.Y) < 0.001)
            {
                velocity.Y = 0;
            }
            Acceleration = Vector2.Zero;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.Instance.debugFont, "onWall: " + onWall +"\nonLeftWall: "+ onLeftWall + "\nRightWall:" + onRightWall+ "\nonGround: " + onGround + "\njumps remaining: " + jumpCount, Position + new Vector2(0, 40), Color.Red);
        }
    }
}
