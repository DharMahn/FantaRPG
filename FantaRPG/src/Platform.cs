using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FantaRPG.src.Interfaces;

namespace FantaRPG.src
{
    internal class Platform : Entity
    {
        public Platform(Texture2D texture, float x, float y, float w, float h, bool collidable = true) : base(texture, x, y, w, h)
        {
            IsCollidable = collidable;
        }
        public virtual void Trigger()
        {

        }
    }
}
