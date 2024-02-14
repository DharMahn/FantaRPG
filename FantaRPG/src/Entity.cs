using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FantaRPG.src
{
    [Serializable]

    internal class Entity : BasicCollision
    {
        public bool IsCollidable { get; protected set; }
        protected Texture2D Texture;
        public Stats Stats;
        protected bool alive = true;
        private Vector2 lastPos;
        public Vector2 LastPosition => lastPos;
        public bool Alive { get => alive; set => alive = value; }
        protected bool controllable = true;
        protected bool gravityAffected = false;
        protected bool collisionAffected = true;
        protected Vector2 acceleration;

        public Entity()
        {

        }
        public Entity(float x, float y, Vector2 size, Texture2D texture = null)
        {
            Texture = texture ?? Game1.Instance.pixel;

            Position = new Vector2(x, y);
            lastPos = new Vector2(x, y);
            HitboxSize = size;
            Stats = new Stats();
            ProcessStats();
        }
        public virtual void ProcessStats()
        {

        }
        public virtual void Update(GameTime gameTime)
        {
            lastPos = position;
            if (gravityAffected)
            {
                acceleration.Y += Game1.Instance.CurrentRoom.Gravity * 2000 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            
            Velocity += acceleration;
            if (collisionAffected)
            {
                foreach (Platform platform in Game1.Instance.CurrentRoom.Platforms)
                {
                    if (!platform.IsCollidable) continue;
                    if (IsTouchingTopOf(platform, gameTime))
                    {
                        position.Y = platform.Position.Y - HitboxSize.Y;
                        velocity.Y = 0;
                        acceleration.Y = 0;
                    }
                    else if (IsTouchingBottomOf(platform, gameTime))
                    {
                        position.Y = platform.Position.Y + platform.HitboxSize.Y;
                        velocity.Y = 0;
                        acceleration.Y = 0;
                    }
                    if (IsTouchingLeftOf(platform, gameTime))
                    {
                        position.X = platform.Position.X - HitboxSize.X;
                        velocity.X = 0;
                        acceleration.X = 0;
                    }
                    else if (IsTouchingRightOf(platform, gameTime))
                    {
                        position.X = platform.Position.X + platform.HitboxSize.X;
                        velocity.X = 0;
                        acceleration.X = 0;
                    }
                }
            }
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            acceleration = Vector2.Zero;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                Rectangle sourceRectangle = new((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y);
                Rectangle destinationRectangle = new((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y);
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }
            if (Game1.Instance.debugFont != null)
            {
                spriteBatch.DrawString(Game1.Instance.debugFont, "{" + Position.X.ToString("0.0") + ";" + Position.Y.ToString("0.0") + "}\n" + "{" + Velocity.X.ToString("0.0") + ";" + Velocity.Y.ToString("0.0") + "}", Position, Color.Red);
            }
            //g.DrawString(Position.ToString("0.0"), SystemFonts.DefaultFont, Brushes.Black, Position.X - 10, Position.Y - 15);
        }
    }
}
