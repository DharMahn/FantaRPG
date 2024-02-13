using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace FantaRPG.src.HUD
{
    internal class MainResourcesDisplay(Player player)
    {
        private readonly Player player = player;

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.Instance.debugFont, player.Stats[Stat.Health].ToString(), new Vector2(), Color.Black);
        }
    }
}
