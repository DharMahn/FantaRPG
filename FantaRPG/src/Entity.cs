using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class Entity : BasicCollision
    {
        protected Texture2D Texture;
        public Stats Stats;
        public Entity(Texture2D texture)
        {
            Texture = texture;
            Position = Vector2.Zero;
            HitboxSize = new Vector2(20, 20);
            Stats = new Stats(); 
            ProcessStats();
        }
        public Entity(Texture2D texture, int x, int y, int w, int h)
        {
            Texture = texture;
            Position = new Vector2(x, y);
            HitboxSize = new Vector2(w, h);
            Stats = new Stats();
            ProcessStats();
        }
        public void ProcessStats()
        {
            Stats.MoveSpeed = 4;
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y), Color.White);
            if (Game1.Instance.debugFont != null)
            {
                spriteBatch.DrawString(Game1.Instance.debugFont, "{" + Position.X.ToString("0.0") + ";" + Position.Y.ToString("0.0") + "}", Position, Color.Black);
            }
            //g.DrawString(Position.ToString("0.0"), SystemFonts.DefaultFont, Brushes.Black, Position.X - 10, Position.Y - 15);
        }
    }
}
