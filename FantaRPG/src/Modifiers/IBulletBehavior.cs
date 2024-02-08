using Microsoft.Xna.Framework;
using System;

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
