using Microsoft.Xna.Framework;
using System;

namespace FantaRPG.src.Modifiers
{
    internal class SplitBehavior(int splitCount) : IBulletBehavior
    {
        private readonly int splitCount = splitCount;
        public int PassCount { get; set; } = 0;

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
            SplitBehavior toreturn = new(splitCount);
            return toreturn;
        }
    }
}
