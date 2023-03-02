using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.HUD
{
    internal class HudDisplay
    {
        private InventoryDisplay invDisplay;
        private MainResourcesDisplay mResDisplay;
        public HudDisplay(Player player)
        {
            invDisplay = new(player);
            mResDisplay = new(player);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            invDisplay.Draw(spriteBatch);
            mResDisplay.Draw(spriteBatch);
        }
    }
}
