using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace FantaRPG.src.HUD
{
    internal class MainResourcesDisplay
    {
        private readonly Player player;

        public MainResourcesDisplay(Player player)
        {
            this.player = player;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.Instance.debugFont, player.Stats.GetStat(Stat.Health).ToString(), new Vector2(), Color.Black);
        }
    }
}
