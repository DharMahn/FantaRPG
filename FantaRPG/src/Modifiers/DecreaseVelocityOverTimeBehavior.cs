using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Modifiers
{
    internal class DecreaseVelocityOverTimeBehavior : IBulletBehavior
    {
        public List<IBulletBehavior> OnVelocityTriggerBehaviors { get; } = new List<IBulletBehavior>();
        public bool Passable => true;

        public void ActOnCollision(object sender, EventArgs e) { /* ... */ }

        public float VelocityLengthTrigger = 5;

        public void Update(Bullet bullet, GameTime gameTime)
        {
            // Decrease the velocity
            bullet.Velocity -= Vector2.One * 5 * gameTime.GetElapsedSeconds(); // Change the calculation as per your requirement

            if (bullet.Velocity.Length() <= VelocityLengthTrigger)
            {
                foreach (var behavior in OnVelocityTriggerBehaviors)
                {
                    behavior.Execute(bullet);
                }
            }
        }

        public void Execute(Bullet bullet)
        {
            // Code to execute if this behavior is triggered by another behavior
        }

        public IBulletBehavior Clone()
        {
            return new DecreaseVelocityOverTimeBehavior();
        }
    }
}
