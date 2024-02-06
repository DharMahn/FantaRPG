using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Modifiers
{
    internal class SplitBehavior : IBulletBehavior
    {
        readonly int splitCount;
        public int PassCount { get; set; } = 0;

        public SplitBehavior(int splitCount)
        {
            this.splitCount = splitCount;
        }

        public void ActOnCollision(object sender, EventArgs e)
        {

        }

        public void Update(Bullet bullet, GameTime gameTime)
        {

        }

        public void Execute(Bullet bullet)
        {
            float splitAngle = 360f / splitCount;
            float offset = (float)RNG.GetDouble() * splitAngle;

            for (int i = 0; i < splitCount; i++)
            {
                Bullet newBullet = new(bullet);
                Vector2 newDirection = Extensions.RotateVector(newBullet.Velocity, (splitAngle * i) + offset);
                newBullet.Velocity = newDirection;
                newBullet.CopyBehaviorsFrom(bullet);
                Game1.Instance.CurrentRoom.AddEntity(newBullet);
            }

            bullet.Alive = false;
        }

        public IBulletBehavior Clone()
        {
            var toreturn = new SplitBehavior(splitCount);
            return toreturn;
        }
    }
}
