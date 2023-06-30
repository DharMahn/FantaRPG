using FantaRPG.src.Interfaces;
using FantaRPG.src.Items;
using FantaRPG.src.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        List<Modifier> modifiers = new();
        public List<Modifier> Modifiers { get { return modifiers; } }
        public Inventory.Inventory Inventory;
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
            velocity.Y = Math.Min(velocity.Y, 2);
            Vector2 movementVector = Vector2.Zero;
            Acceleration.Y += 2000 * (float)gameTime.GetElapsedSeconds();
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
                actualMovementVector.X *= Stats.GetStat(Stat.MoveSpeed) * 100f * (float)gameTime.GetElapsedSeconds();
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
            if (MovementInput.MouseLeftJustDown())
            {
                //Debug.WriteLine("clicked");
                Vector2 playerCenter = new(Bounds.Position.X + HitboxSize.X / 2, Bounds.Position.Y + HitboxSize.Y / 2);
                Vector2 cursorPos = new(Mouse.GetState().Position.X - Game1.Instance.cam.Transform.Translation.X, Mouse.GetState().Position.Y - Game1.Instance.cam.Transform.Translation.Y);

                //Debug.WriteLine(cursorPos.X.ToString("0.0") + ";" + cursorPos.Y.ToString("0.0") + " cursorpos, " + Position.X.ToString("0.0") + ";" + Position.Y.ToString("0.0") + " playerpos");
                Vector2 spellVel = new(cursorPos.X - playerCenter.X, cursorPos.Y - playerCenter.Y);
                spellVel.Normalize();
                spellVel = Vector2.Multiply(spellVel, 5000);
                Bullet bullet = new((int)(playerCenter.X - spellSize / 2), (int)(playerCenter.Y - spellSize / 2), new Vector2(spellSize), spellVel, Stats.GetStat(Stat.Damage));
                //bullet.OnCollision += delegate
                //{
                //    Game1.Instance.CurrentRoom.AddEntity(new Bullet(bullet.Bounds.Position.X, bullet.Bounds.Position.Y, new Vector2(bullet.HitboxSize.X, bullet.HitboxSize.Y), Vector2.Normalize(bullet.LastPosition-bullet.Position)*bullet.Velocity.Length(), 10, Game1.Instance.pixel));
                //};
                Game1.Instance.CurrentRoom.AddEntity(bullet);
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
            if (Math.Sign(actualMovementVector.X) == 0 && onGround)
            {
                float drag = 50 * Stats.GetStat(Stat.MoveSpeed) * (float)gameTime.GetElapsedSeconds();
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
            Bounds.Position += Vector2.Multiply(Velocity, (float)gameTime.GetElapsedSeconds());
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
        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (((Entity)collisionInfo.Other).IsCollidable)
            {
                //hitting the top of sth else
                if (collisionInfo.PenetrationVector.Y > 0)
                {
                    position.Y -= collisionInfo.PenetrationVector.Y;
                    velocity.Y = 0;
                    onGround = true;
                    canJump = true;
                    jumpCount = jumpCountMax;
                    onLeftWall = false;
                    onRightWall = false;
                    onWall = false;
                }
                else if (collisionInfo.PenetrationVector.Y < 0)
                {
                    position.Y -= collisionInfo.PenetrationVector.Y;
                    velocity.Y = 0;
                }
                if (collisionInfo.PenetrationVector.X < 0)
                {
                    position.X -= collisionInfo.PenetrationVector.X;
                    velocity.X = 0;
                    onWall = true;
                    onLeftWall = true;
                    onRightWall = false;
                    canJump = true;
                    onWallPosition = position.X;
                }
                else if (collisionInfo.PenetrationVector.X > 0)
                {
                    position.X -= collisionInfo.PenetrationVector.X;
                    velocity.X = 0;
                    onWall = true;
                    onRightWall = true;
                    onLeftWall = false;
                    canJump = true;
                    onWallPosition = position.X;
                }
            }
            else
            {
                if (collisionInfo.Other is Portal portal)
                {
                    portal.ChangeRoom();
                }
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
            spriteBatch.DrawString(Game1.Instance.debugFont, "onWall: " + onWall + "\nonLeftWall: " + onLeftWall + "\nRightWall: " + onRightWall + "\nonGround: " + onGround + "\njumps remaining: " + jumpCount, Bounds.Position + new Vector2(0, 40), Color.Red);
        }
    }
}
