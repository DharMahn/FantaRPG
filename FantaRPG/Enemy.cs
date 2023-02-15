using FantaRPG.src;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG
{
    internal class Enemy : Entity
    {
        public Enemy(Texture2D texture, int x, int y, int w, int h) : base(texture, x, y, w, h)
        {
            Stats = new Stats();
        }
        public override void Update(GameTime gameTime)
        {

            //base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
