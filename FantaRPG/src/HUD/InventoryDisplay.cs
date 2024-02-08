using FantaRPG.src.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FantaRPG.src.HUD
{
    internal class InventoryDisplay(Player player)
    {
        private readonly Player player = player;
        private Rectangle destRect = new();

        public void Draw(SpriteBatch spriteBatch)
        {
            List<Item> playerItems = player.GetInventory();
            int center = Game1.Instance._graphics.PreferredBackBufferWidth / 2;
            int rectSize = center / 16;
            destRect.Width = rectSize;
            destRect.Height = rectSize;
            int height = Game1.Instance._graphics.PreferredBackBufferHeight;
            destRect.Y = height - rectSize - (rectSize / 8);
            for (int i = 0; i < playerItems.Count; i++)
            {
                Item item = playerItems[i];
                destRect.X = center - (playerItems.Count / 2 * rectSize) + (i * rectSize);
                spriteBatch.Draw(item.Texture, destRect, item.Tint);
                spriteBatch.DrawString(
                    Game1.Instance.debugFont,
                    item.Name,
                    new Vector2(destRect.X, destRect.Y + (destRect.Height * 1.1f)),
                    Color.Black);
            }
        }
    }
}
