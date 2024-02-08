using Microsoft.Xna.Framework.Graphics;

namespace FantaRPG.src.HUD
{
    internal class HudDisplay(Player player)
    {
        private readonly InventoryDisplay invDisplay = new(player);
        private readonly MainResourcesDisplay mResDisplay = new(player);

        public void Draw(SpriteBatch spriteBatch)
        {
            invDisplay.Draw(spriteBatch);
            mResDisplay.Draw(spriteBatch);
        }
    }
}
