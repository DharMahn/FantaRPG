using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG
{
    internal class BackgroundLayer
    {
        public Texture2D Texture;
        public int LayerID;
        public BackgroundLayer(Texture2D texture,int layerid)
        {
            Texture = texture;
            LayerID = layerid;
        }
        public void Draw(SpriteBatch spriteBatch, Camera c)
        {

            spriteBatch.Draw(Texture,
                new Rectangle(
                    (int)0,
                    (int)0,
                    (int)(Texture.Width * Game1.Instance.Ratio),
                    (int)(Texture.Height * Game1.Instance.Ratio)),
                    new Rectangle(-(int)c.Transform.Translation.X / LayerID, 0, Texture.Width, Texture.Height),
                    Color.White);
        }
    }
}
