using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantaRPG.src.Items;

namespace FantaRPG.src
{
    internal class Entity : BasicCollision
    {
        public bool IsCollidable { get; protected set; }
        protected Texture2D Texture;
        public Stats Stats;
        protected bool alive = true;
        public bool Alive { get { return alive; } }

        public Entity(float x, float y, float w, float h, Texture2D texture = null)
        {
            if (texture==null)
            {
                Texture = Game1.Instance.pixel;
            }
            Position = new Vector2(x, y);
            HitboxSize = new Vector2(w, h);
            Stats = new Stats();
            ProcessStats();
        }
        public virtual void ProcessStats()
        {

        }
        public virtual void Update(GameTime gameTime)
        {
            Position += Velocity;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(Texture, Position, new Rectangle((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y), Color.White);
            }
            if (Game1.Instance.debugFont != null)
            {
                spriteBatch.DrawString(Game1.Instance.debugFont, "{" + Position.X.ToString("0.0") + ";" + Position.Y.ToString("0.0") + "}\n"+ "{" + Velocity.X.ToString("0.0") + ";" + Velocity.Y.ToString("0.0") + "}", Position, Color.Red);
            }
            //g.DrawString(Position.ToString("0.0"), SystemFonts.DefaultFont, Brushes.Black, Position.X - 10, Position.Y - 15);
        }
    }
}
