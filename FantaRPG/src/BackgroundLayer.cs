using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FantaRPG.src
{
    internal class BackgroundLayer(Texture2D texture, int layerid)
    {
        public Texture2D Texture = texture;
        public int LayerID = layerid;

        public void Draw(SpriteBatch spriteBatch, Matrix transform)
        {

            spriteBatch.Draw(Texture,
                new Rectangle(
                    0,
                    0,
                    (int)(Texture.Width * Game1.Instance.Ratio),
                    (int)(Texture.Height * Game1.Instance.Ratio)),
                    new Rectangle(-(int)transform.Translation.X / LayerID, 0, Texture.Width, Texture.Height),
                    Color.White);
        }
    }
}
