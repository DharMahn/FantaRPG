using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FantaRPG.src.Interfaces
{
    internal interface IAnimation
    {
        public void Draw(SpriteBatch spriteBatch, Camera cam);
        public void Update(GameTime gameTime);
    }
}
