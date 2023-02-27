using FantaRPG.src;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Enemies
{
    internal class WalkerEnemy : Entity
    {
        public WalkerEnemy(float x, float y, float w, float h, Texture2D texture = null) : base(x, y, w, h, texture)
        {

        }
    }
}
