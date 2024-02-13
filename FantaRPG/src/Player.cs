using FantaRPG.src.Items;
using FantaRPG.src.Modifiers;
using FantaRPG.src.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace FantaRPG.src
{
    internal class Player : Entity
    {
        private readonly float wallJumpVelX = 750f;
        private readonly Dictionary<string, Keys> Input;
        private float lastCooldownTime = 0;
        private readonly int spellSize = 10;
        private bool onGround = false;
        private bool canJump = true;
        private int jumpCount = 1;
        private readonly int jumpCountMax = 1;
        private bool onWall = false;
        private bool onLeftWall;
        private bool onRightWall;

        public List<Modifier> Modifiers { get; } = [];
        public Inventory.Inventory Inventory;
        private readonly Item selectedItem = new("TEST ITEM");
        private readonly BasicCollision leftSideTrigger;
        private readonly BasicCollision rightSideTrigger;
        public List<Item> GetInventory()
        {
            return Inventory.GetItems();
        }
        public bool AddItem(Item item)
        {
            Inventory.AddItem(item);
            return true;
        }
        public Player(Dictionary<string, Keys> input, float x, float y, Vector2 size, Texture2D texture = null) : base(x, y, size, texture)
        {
            Input = input;
            Inventory = new Inventory.Inventory();
            leftSideTrigger = new BasicCollision
            {
                HitboxSize = new Vector2(3, size.Y - 2),
                Position = new Vector2(x - 2, y + 1),
            };
            rightSideTrigger = new BasicCollision
            {
                HitboxSize = new Vector2(3, size.Y - 2),
                Position = new Vector2(x + size.X - 1, y + 1),
            };
            gravityAffected = true;
        }
        public override void Update(GameTime gameTime)
        {
            Vector2 movementVector = Vector2.Zero;
            if (gravityAffected)
            {
                acceleration.Y += Game1.Instance.CurrentRoom.Gravity * 2000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
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
                    onLeftWall = false;
                }
            }
            if (MovementInput.KeyDown(Input["Right"]))
            {
                movementVector += Vector2.UnitX;
                if (onWall && onRightWall)
                {
                    onWall = false;
                    onRightWall = false;
                }
            }
            Vector2 actualMovementVector = Vector2.Zero;
            if (movementVector != Vector2.Zero)
            {
                actualMovementVector = Vector2.Normalize(movementVector);
                actualMovementVector.X *= Stats[Stat.MoveSpeed] * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            #region No Wall Slide
            //if (onWall)
            //{
            //    velocity.Y = 0;
            //    Acceleration.Y = 0;
            //}
            #endregion
            if (MovementInput.KeyDown(Input["Jump"]))
            {
                if (onWall && canJump)
                {
                    if (onLeftWall)
                    {
                        velocity.X = (-35 * Stats[Stat.JumpStrength]);
                        velocity.Y = -50 * Stats[Stat.JumpStrength];
                        onLeftWall = false;
                        onWall = false;
                    }
                    else if (onRightWall)
                    {
                        velocity.X = -(-35 * Stats[Stat.JumpStrength]);
                        velocity.Y = -50 * Stats[Stat.JumpStrength];
                        onRightWall = false;
                        onWall = false;
                    }
                    else
                    {
                        velocity.Y = -50 * Stats[Stat.JumpStrength];
                    }
                    canJump = false;
                    jumpCount = 0;
                }
                else
                {
                    if (jumpCount > 0)
                    {
                        jumpCount--;
                        velocity.Y = -50 * Stats[Stat.JumpStrength];
                        onGround = false;
                    }
                }
            }
            float elapsedSeconds = (float)gameTime.GetElapsedSeconds(); // Gets the time elapsed since the last update
            lastCooldownTime += elapsedSeconds; // Increment the time since the last bullet
            if (lastCooldownTime > selectedItem.Cooldown)
            {
                lastCooldownTime = selectedItem.Cooldown;
            }

            //spawn a bullet if enough time has been spent and we are shooting
            if (MovementInput.MouseLeftDown() && lastCooldownTime >= selectedItem.Cooldown)
            {
                lastCooldownTime -= selectedItem.Cooldown; // Reset the timer since a bullet was just fired
                Vector2 playerCenter = new(Position.X + (HitboxSize.X / 2), Position.Y + (HitboxSize.Y / 2));
                Vector2 cursorPos = new(Mouse.GetState().Position.X - Game1.Instance.cam.Transform.Translation.X, Mouse.GetState().Position.Y - Game1.Instance.cam.Transform.Translation.Y);
                
                //placeholder initialization until i make the items have stats and bullets
                Vector2 spellVel = new(cursorPos.X - playerCenter.X, cursorPos.Y - playerCenter.Y);
                spellVel.Normalize();
                spellVel = Vector2.Multiply(spellVel, 1000);
                spellVel += Velocity;
                Bullet bullet = new((int)(playerCenter.X - (spellSize / 2)), (int)(playerCenter.Y - (spellSize / 2)), new Vector2(spellSize), spellVel, Stats[Stat.Damage], this);
                DecreaseVelocityOverTimeBehavior modifier = new(1f, 200);
                modifier.OnVelocityTriggerBehaviors.Add(new SplitBehavior(7));
                bullet.AddBehavior(modifier);
                float curve = 120f * (((float)RNG.GetDouble() * 2) - 1);
                bullet.AddBehavior(new CurvedVelocityBehavior(curve));

                Game1.Instance.CurrentRoom.AddEntity(bullet);
            }
            acceleration += actualMovementVector;

            //the first part is a condition to check against overacceleration,
            //but it doesn't outright limit the velocity, it just limits further acceleration
            //but what the second part does is
            //let's say our velocity is 100, our movespeed is 1, so multiply that by 10, it becomes 10,
            //even if the acceleration is -10, it would only decrease the velocity to 90,
            //which would be larger than 10, therefore not applied, that's why we need the second condition,
            //so we can counteract a heavy external force's effect on the player, so you don't get slammed against a wall
            //without being able to do anything(although it sounds like a fun thing now that i wrote it down)
            if (Math.Abs(velocity.X + acceleration.X) < Stats[Stat.MoveSpeed] * 10 || Math.Sign(acceleration.X) != Math.Sign(velocity.X))
            {
                velocity.X += acceleration.X;
            }
            velocity.Y += acceleration.Y;
            onGround = false;
            foreach (Portal portal in Game1.Instance.CurrentRoom.Portals)
            {
                if (IsTouching(portal, gameTime))
                {
                    portal.ChangeRoom();
                }
            }
            onLeftWall = false;
            onRightWall = false;
            onWall = false;
            foreach (Platform platform in Game1.Instance.CurrentRoom.Platforms)
            {
                if (!platform.IsCollidable)
                {
                    continue;
                }

                if (rightSideTrigger.IsTouchingLeftOf(platform, gameTime))
                {
                    onLeftWall = true;
                    onWall = true;
                    canJump = true;
                }
                if (leftSideTrigger.IsTouchingRightOf(platform, gameTime))
                {
                    onRightWall = true;
                    onWall = true;
                    canJump = true;
                }
                if (IsTouchingTopOf(platform, gameTime))
                {
                    position.Y = platform.Position.Y - HitboxSize.Y;
                    velocity.Y = 0;
                    onGround = true;
                    canJump = true;
                    jumpCount = jumpCountMax;
                    onLeftWall = false;
                    onRightWall = false;
                    onWall = false;
                }
                else if (IsTouchingBottomOf(platform, gameTime))
                {
                    position.Y = platform.Position.Y + platform.HitboxSize.Y;
                    velocity.Y = 0;
                }
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
            if (Math.Sign(actualMovementVector.X) == 0 && onGround)
            {
                float drag = 50 * Stats[Stat.MoveSpeed] * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            // For leftSideTrigger
            leftSideTrigger.Position = new Vector2(Position.X - 1, Position.Y + 1);

            // For rightSideTrigger
            rightSideTrigger.Position = new Vector2(Position.X + HitboxSize.X - 2, Position.Y + 1);

            if (Math.Abs(Velocity.X) < 0.001)
            {
                velocity.X = 0;
            }
            if (Math.Abs(Velocity.Y) < 0.001)
            {
                velocity.Y = 0;
            }
            acceleration = Vector2.Zero;
        }
        public override void ProcessStats()
        {
            base.ProcessStats();
            Stats[Stat.MoveSpeed] = 40;
            Stats[Stat.JumpStrength] = 20;
            foreach (Modifier modifier in Modifiers)
            {
                foreach (KeyValuePair<Stat, float> stat in modifier.Stats.GetAllStats())
                {
                    Stats[stat.Key] += stat.Value;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, new Rectangle((int)leftSideTrigger.Position.X, (int)leftSideTrigger.Position.Y, (int)leftSideTrigger.HitboxSize.X, (int)leftSideTrigger.HitboxSize.Y), Color.Red);
            spriteBatch.Draw(Texture, new Rectangle((int)rightSideTrigger.Position.X, (int)rightSideTrigger.Position.Y, (int)rightSideTrigger.HitboxSize.X, (int)rightSideTrigger.HitboxSize.Y), Color.Blue);
            if (Game1.Instance.debugFont != null)
            {
                spriteBatch.DrawString(Game1.Instance.debugFont, "onWall: " + onWall + "\nonLeftWall: " + onLeftWall + "\nonRightWall: " + onRightWall + "\nonGround: " + onGround + "\njumps remaining: " + jumpCount, Position + new Vector2(0, 40), Color.Red);
            }
        }
    }
}
