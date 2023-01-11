using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG
{
    internal class Entity
    {
        public Vector2 Position;
        private Texture2D texture;
        public Entity(Texture2D texture)
        {
            this.texture = texture;
            Position = Vector2.Zero;
        }
        public Entity(Texture2D texture, int x, int y)
        {
            this.texture = texture;
            Position = new Vector2(x, y);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position,new Rectangle((int)Position.X,(int)Position.Y,20,20), Color.White);
            if (Game1.Instance.debugFont!=null)
            {
                spriteBatch.DrawString(Game1.Instance.debugFont, "{" + Position.X + ";" + Position.Y + "}", Position, Color.Black);
            }
            //g.DrawString(Position.ToString("0.0"), SystemFonts.DefaultFont, Brushes.Black, Position.X - 10, Position.Y - 15);
        }
    }
}
