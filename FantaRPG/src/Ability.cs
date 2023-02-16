using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class Ability : Bullet
    {


        public Ability(Texture2D texture, int x, int y, int w, int h, Vector2 velocity) : base(texture, x, y, w, h, velocity)
        {

        }
        public new void Update(GameTime gameTime)
        {
            foreach (var item in Game1.Instance.CurrentRoom.Objects)
            {
                if (IsTouchingLeft(item, gameTime) || IsTouchingRight(item, gameTime) || IsTouchingTop(item, gameTime) || IsTouchingBottom(item, gameTime))
                {

                }
            }
            base.Update(gameTime);
        }
    }
}
