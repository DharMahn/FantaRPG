using FantaRPG.src;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Enemies
{
    internal class WalkerEnemy : Entity
    {
        public WalkerEnemy(Texture2D texture, float x, float y, float w, float h) : base(texture, x, y, w, h)
        {

        }
        public override void Update(GameTime gameTime)
        {
            //Game1.Instance.CurrentRoom.Player.Position;
            //base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
