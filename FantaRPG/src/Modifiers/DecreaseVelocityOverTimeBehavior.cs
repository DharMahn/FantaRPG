﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
namespace FantaRPG.src.Modifiers
{
    internal class DecreaseVelocityOverTimeBehavior(float duration, float velocityLengthTrigger) : IBulletBehavior
    {
        public List<IBulletBehavior> OnVelocityTriggerBehaviors { get; } = [];
        public int PassCount { get; set; } = 0;
        public void ActOnCollision(object sender, EventArgs e) { /* ... */ }

        public float VelocityLengthTrigger = velocityLengthTrigger;
        private static readonly float epsilon = 0.01f;
        private readonly float duration = duration;
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
                foreach (IBulletBehavior behavior in OnVelocityTriggerBehaviors)
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
            DecreaseVelocityOverTimeBehavior cloned = new(duration, 0);
            foreach (IBulletBehavior item in OnVelocityTriggerBehaviors)
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
