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
        public BackgroundLayer(Texture2D texture)
        {
            Texture = texture;
        }
        public void Draw(SpriteBatch spriteBatch, Camera c)
        {
            spriteBatch.Draw(Texture, c.Position/LayerID,Color.White);
        }
    }
}
