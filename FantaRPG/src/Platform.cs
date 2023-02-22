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
        public Platform(float x, float y, float w, float h, Texture2D texture = null, bool collidable = true) : base(x, y, w, h, texture)
        {
            if (texture == null)
            {
                texture = Game1.Instance.pixel;
            }
            
            IsCollidable = collidable;
        }
        public virtual void Trigger()
        {

        }
    }
}
