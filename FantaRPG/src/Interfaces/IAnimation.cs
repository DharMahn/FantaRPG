using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Interfaces
{
    internal interface IAnimation
    {
        public void Draw(SpriteBatch spriteBatch, Camera cam);
        public void Update(GameTime gameTime);
    }
}
