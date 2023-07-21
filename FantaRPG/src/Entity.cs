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
        private Vector2 lastPos;
        public Vector2 LastPosition { get { return lastPos; } }
        public bool Alive { get { return alive; } set { alive = value; } }

        public Entity(float x, float y, Vector2 size, Texture2D texture = null)
        {
            if (texture==null)
            {
                Texture = Game1.Instance.pixel;
            }
            else
            {
                Texture = texture;
            }
            
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
            Position += Velocity*(float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                Rectangle sourceRectangle = new Rectangle((int)Position.X, (int)Position.Y,(int)HitboxSize.X, (int)HitboxSize.Y);
                Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y);
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }
            if (Game1.Instance.debugFont != null)
            {
                spriteBatch.DrawString(Game1.Instance.debugFont, "{" + Position.X.ToString("0.0") + ";" + Position.Y.ToString("0.0") + "}\n"+ "{" + Velocity.X.ToString("0.0") + ";" + Velocity.Y.ToString("0.0") + "}", Position, Color.Red);
            }
            //g.DrawString(Position.ToString("0.0"), SystemFonts.DefaultFont, Brushes.Black, Position.X - 10, Position.Y - 15);
        }
    }
}
