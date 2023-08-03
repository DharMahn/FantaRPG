using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Modifiers
{
    internal class SplitIntoTwoBehavior : IBulletBehavior
    {
        float splitAngle = 10f;
        public bool Passable => false;
        public void ActOnCollision(object sender, EventArgs e) { /* ... */ }

        public void Update(Bullet bullet, GameTime gameTime) { /* ... */ }

        public void Execute(Bullet bullet)
        {

            Bullet bullet1 = new Bullet(bullet);
            Bullet bullet2 = new Bullet(bullet);

            Vector2 newDirection1 = Extensions.RotateVector(bullet1.Velocity, splitAngle);
            Vector2 newDirection2 = Extensions.RotateVector(bullet2.Velocity, -splitAngle);

            bullet1.Velocity = newDirection1;
            bullet2.Velocity = newDirection2;

            bullet1.CopyBehaviorsFrom(bullet);
            bullet2.CopyBehaviorsFrom(bullet);

            Game1.Instance.CurrentRoom.AddEntity(bullet1);
            Game1.Instance.CurrentRoom.AddEntity(bullet2);

            bullet.Alive = false;
        }

        public IBulletBehavior Clone()
        {
            return new SplitIntoTwoBehavior();
        }
    }
}
