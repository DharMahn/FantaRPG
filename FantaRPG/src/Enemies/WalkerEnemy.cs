using FantaRPG.src;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Enemies
{
    internal class WalkerEnemy : AIEnemy
    {
        public WalkerEnemy(float x, float y, Vector2 size, Texture2D texture = null) : base(x, y, size, texture)
        {
            damageMultiplier = 1f;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
