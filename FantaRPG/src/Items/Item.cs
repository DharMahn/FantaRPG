using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FantaRPG.src.Items
{
    internal class Item
    {
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public Color Tint;
        public float Cooldown { get; private set; }
        public Item(string name, Texture2D texture = null, float cooldown = 0.25f)
        {
            Name = name;
            texture ??= Game1.Instance.pixel;
            Texture = texture;
            Tint = Color.White;
            Cooldown = cooldown;
        }
    }
}
