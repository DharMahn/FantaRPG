﻿using FantaRPG.src.Interfaces;
using FantaRPG.src.Items;
using FantaRPG.src.Modifiers;
using FantaRPG.src.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class Player : Entity
    {
        private float wallJumpVelX = 750f;
        private float wallJumpVelY = 1250f;
        private Dictionary<string, Keys> Input;
        private Vector2 Acceleration;
        private float lastCooldownTime = 0;
        int spellSize = 10;
        bool onGround = false;
        bool canJump = true;
        int jumpCount = 1;
        int jumpCountMax = 1;
        bool onWall = false;
        bool onLeftWall;
        bool onRightWall;
        List<Modifier> modifiers = new();
        public List<Modifier> Modifiers { get { return modifiers; } }
        public Inventory.Inventory Inventory;
        private Item selectedItem = new Item("TEST ITEM");
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
        }
        float onWallPosition = 0;
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
                actualMovementVector.X *= Stats.GetStat(Stat.MoveSpeed) * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (MovementInput.KeyDown(Input["Jump"]))
            {
                if (onWall && canJump)
                {
                    if (onLeftWall)
                    {
                        velocity.X = -wallJumpVelX;
                        velocity.Y = -wallJumpVelY;
                        onLeftWall = false;
                        onWall = false;
                    }
                    else if (onRightWall)
                    {
                        velocity.X = wallJumpVelX;
                        velocity.Y = -wallJumpVelY;
                        onRightWall = false;
                        onWall = false;
                    }
                    else
                    {
                        velocity.Y = -50 * Stats.GetStat(Stat.JumpStrength);
                    }
                    canJump = false;
                    jumpCount = 0;
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
            float elapsedSeconds = (float)gameTime.GetElapsedSeconds(); // Gets the time elapsed since the last update
            lastCooldownTime += elapsedSeconds; // Increment the time since the last bullet
            if (lastCooldownTime > selectedItem.Cooldown)
            {
                lastCooldownTime = selectedItem.Cooldown;
            }
            if (MovementInput.MouseLeftDown() && lastCooldownTime >= selectedItem.Cooldown)
            {
                lastCooldownTime -= selectedItem.Cooldown; // Reset the timer since a bullet was just fired

                Vector2 playerCenter = new Vector2(Position.X + HitboxSize.X / 2, Position.Y + HitboxSize.Y / 2);
                Vector2 cursorPos = new Vector2(Mouse.GetState().Position.X - Game1.Instance.cam.Transform.Translation.X, Mouse.GetState().Position.Y - Game1.Instance.cam.Transform.Translation.Y);

                Vector2 spellVel = new Vector2(cursorPos.X - playerCenter.X, cursorPos.Y - playerCenter.Y);
                spellVel.Normalize();
                spellVel = Vector2.Multiply(spellVel, 500);
                Bullet bullet = new Bullet((int)(playerCenter.X - spellSize / 2), (int)(playerCenter.Y - spellSize / 2), new Vector2(spellSize), spellVel, Stats.GetStat(Stat.Damage), null);
                var modifier = new DecreaseVelocityOverTimeBehavior(0.5f);
                modifier.OnVelocityTriggerBehaviors.Add(new SplitBehavior(11));
                bullet.AddBehavior(modifier);
                Game1.Instance.CurrentRoom.AddEntity(bullet);
            }
            if (/*onGround*/true)
            {
                Acceleration += actualMovementVector;
                if (Math.Abs(velocity.X + Acceleration.X) < Stats.GetStat(Stat.MoveSpeed) * 10 || Math.Sign(Acceleration.X) != Math.Sign(velocity.X))
                {
                    velocity.X += Acceleration.X;
                }
                //else
                //{
                //    if (velocity.X + Acceleration.X > 0)
                //    {
                //        velocity.X = Stats.GetStat(Stat.MoveSpeed) * 10;
                //    }
                //    else
                //    {
                //        velocity.X = -Stats.GetStat(Stat.MoveSpeed) * 10;
                //    }
                //}
                //velocity.X = Math.Sign(velocity.X) == Math.Sign(Acceleration.X) ? Velocity.X + Acceleration.X : (Velocity.X / 2) + Acceleration.X;
                if (onGround)
                {
                    //velocity.X = Math.Clamp(velocity.X, -Stats.GetStat(Stat.MoveSpeed) * 10, Stats.GetStat(Stat.MoveSpeed) * 10);
                }
                velocity.Y += Acceleration.Y;
            }
            onGround = false;
            foreach (var portal in Game1.Instance.CurrentRoom.Portals)
            {
                if (IsTouchingLeft(portal, gameTime) ||
                    IsTouchingRight(portal, gameTime) ||
                    IsTouchingTop(portal, gameTime) ||
                    IsTouchingBottom(portal, gameTime))
                {
                    portal.ChangeRoom();
                }
            }
            foreach (var item in Game1.Instance.CurrentRoom.Platforms)
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
                        onWall = false;
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
                        onWallPosition = position.X;
                    }
                    else if (IsTouchingRight(item, gameTime))
                    {
                        position.X = item.Position.X + item.HitboxSize.X;
                        velocity.X = 0;
                        onWall = true;
                        onRightWall = true;
                        onLeftWall = false;
                        canJump = true;
                        onWallPosition = position.X;
                    }
                }
            }
            if (Math.Sign(actualMovementVector.X) == 0 && onGround)
            {
                float drag = 50 * Stats.GetStat(Stat.MoveSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            if (onWallPosition != position.X)
            {
                onWall = false;
                onLeftWall = false;
                onRightWall = false;
            }
        }
        public void AddModifier(Modifier modifier)
        {
            modifiers.Add(modifier);
        }
        public void RemoveModifier(Modifier modifier)
        {
            modifiers.Remove(modifier);
        }
        public override void ProcessStats()
        {
            base.ProcessStats();
            Stats.SetStat(Stat.MoveSpeed, 40);
            Stats.SetStat(Stat.JumpStrength, 20);
            foreach (var modifier in Modifiers)
            {
                foreach (var stat in modifier.Stats.GetAllStats())
                {
                    Stats.IncrementStat(stat.Key, stat.Value);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (Game1.Instance.debugFont != null)
            {
                spriteBatch.DrawString(Game1.Instance.debugFont, "onWall: " + onWall + "\nonLeftWall: " + onLeftWall + "\nRightWall: " + onRightWall + "\nonGround: " + onGround + "\njumps remaining: " + jumpCount, Position + new Vector2(0, 40), Color.Red);
            }
        }
    }
}
