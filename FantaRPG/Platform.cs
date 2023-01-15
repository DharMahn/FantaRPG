using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FantaRPG
{
    internal class Platform : Entity
    {
        public Platform(Texture2D texture, int x, int y, int w, int h) : base(texture)
        {
            Position=new Vector2(x, y);
            HitboxSize= new Vector2(w, h);
        }
    }
}
