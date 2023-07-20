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
        public Platform(float x, float y, Vector2 size, Texture2D texture = null, bool collidable = true) : base(x, y, size, texture)
        {
            IsCollidable = collidable;
            Texture = Game1.Instance.Content.Load<Texture2D>("wall0");
        }
        public virtual void Trigger()
        {

        }
    }
}
