using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Items
{
    internal class Item
    {
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public Color Tint;
        public Item(string name, Texture2D texture = null)
        {
            Name = name;
            if (texture == null)
            {
                texture = Game1.Instance.pixel;
            }
            else
            {
                Texture = texture;
            }
            Tint = Color.White;
        }
    }
}
