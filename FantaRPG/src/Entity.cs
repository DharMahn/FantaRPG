using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantaRPG.src.Items;
using MonoGame.Extended;

namespace FantaRPG.src
{
    internal class Entity : BasicCollision
    {
        public bool IsCollidable { get; protected set; }
        protected Texture2D Texture;
        public Stats Stats;
        protected bool alive = true;
        private Vector2 lastPos;
        public Vector2 LastPosition { get { return lastPos; } }
        public bool Alive { get { return alive; } }
        protected float damageMultiplier = 0f;
        public Entity(float x, float y, Vector2 size, Texture2D texture = null)
        {
            if (texture == null)
            {
                Texture = Game1.Instance.pixel;
            }
            else
            {
                Texture = texture;
            }
            Bounds = new RectangleF(x, y, size.X, size.Y);
            lastPos = new Vector2(x, y);
            Stats = new Stats();
            AddBasicStats();
            ProcessStats();
        }
        public virtual void ProcessStats()
        {

        }
        public virtual void Damage(float damage)
        {
            Stats.IncrementStat(Stat.Health, damage * damageMultiplier);
        }
        protected virtual void AddBasicStats()
        {
            Stats.SetStat(Stat.Health, 100);
            Stats.SetStat(Stat.Mana, 100);
        }
        public virtual void Update(GameTime gameTime)
        {
            lastPos = position;
            Bounds.Position += Velocity * (float)gameTime.GetElapsedSeconds();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(Texture, Bounds.Position, new Rectangle((int)Bounds.Position.X, (int)Bounds.Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y), Color.White);
            }
            if (Game1.Instance.debugFont != null)
            {
                spriteBatch.DrawString(Game1.Instance.debugFont, "{" + Bounds.Position.X.ToString("0.0") + ";" + Bounds.Position.Y.ToString("0.0") + "}\n" + "{" + Velocity.X.ToString("0.0") + ";" + Velocity.Y.ToString("0.0") + "}", Bounds.Position, Color.Red);
            }
            //g.DrawString(Position.ToString("0.0"), SystemFonts.DefaultFont, Brushes.Black, Position.X - 10, Position.Y - 15);
        }
    }
}
