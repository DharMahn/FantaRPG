using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Modifiers
{
    internal interface IBulletBehavior
    {
        void ActOnCollision(object sender, EventArgs e);
        void Update(Bullet bullet, GameTime gameTime);
        void Execute(Bullet bullet);
        int PassCount { get; set; }
        IBulletBehavior Clone();
    }
}
