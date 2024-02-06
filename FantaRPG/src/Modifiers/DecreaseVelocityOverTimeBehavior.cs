using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tweening;
namespace FantaRPG.src.Modifiers
{
    internal class DecreaseVelocityOverTimeBehavior : IBulletBehavior
    {
        public List<IBulletBehavior> OnVelocityTriggerBehaviors { get; } = new List<IBulletBehavior>();
        public int PassCount { get; set; } = 0;
        public void ActOnCollision(object sender, EventArgs e) { /* ... */ }

        public float VelocityLengthTrigger = 5;
        private static float epsilon = 0.01f;
        private float duration;
        //Tweener tweener = null;
        public DecreaseVelocityOverTimeBehavior(float duration, float velocityLengthTrigger)
        {
            this.duration = duration;
            VelocityLengthTrigger = velocityLengthTrigger;
        }
        private bool decelerationInitialized = false;
        private Vector2 initialVelocity;
        private float decelerationRatePerSecond;
        public void Update(Bullet bullet, GameTime gameTime)
        {
            if (!decelerationInitialized)
            {
                // Calculate the initial deceleration rate needed
                initialVelocity = bullet.Velocity;
                float totalDecelerationNeeded = initialVelocity.Length() - VelocityLengthTrigger;
                decelerationRatePerSecond = totalDecelerationNeeded / duration;
                decelerationInitialized = true;
            }

            // Calculate the amount to decelerate this frame
            float decelerationThisFrame = decelerationRatePerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate the new velocity length
            float newVelocityLength = bullet.Velocity.Length() - decelerationThisFrame;

            if (newVelocityLength <= VelocityLengthTrigger)
            {
                // If the new velocity is less than the target, set the velocity to the target
                bullet.Velocity = Vector2.Normalize(bullet.Velocity) * VelocityLengthTrigger;
                // Optionally, trigger behaviors and mark the bullet for removal
                foreach (var behavior in OnVelocityTriggerBehaviors)
                {
                    behavior.Execute(bullet);
                }
                bullet.Alive = false;
            }
            else
            {
                // Update the bullet's velocity to maintain direction but reduce magnitude
                bullet.Velocity = Vector2.Normalize(bullet.Velocity) * newVelocityLength;
            }
        }

        public void Execute(Bullet bullet)
        {
            // Code to execute if this behavior is triggered by another behavior
        }

        public IBulletBehavior Clone()
        {
            DecreaseVelocityOverTimeBehavior cloned = new DecreaseVelocityOverTimeBehavior(duration, 0);
            foreach (var item in OnVelocityTriggerBehaviors)
            {
                if (item.PassCount > 0)
                {
                    cloned.OnVelocityTriggerBehaviors.Add(item);
                }
            }
            return cloned;
        }
    }
}
