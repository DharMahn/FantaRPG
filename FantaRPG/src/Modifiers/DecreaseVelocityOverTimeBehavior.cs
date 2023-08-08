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
        public float Duration { get { return duration; } }
        Tweener tweener = null;
        public DecreaseVelocityOverTimeBehavior(float duration, float velocityLengthTrigger)
        {
            this.duration = duration;
            VelocityLengthTrigger = velocityLengthTrigger;
        }

        public void Update(Bullet bullet, GameTime gameTime)
        {
            if (tweener == null)
            {
                tweener = new Tweener();
                tweener.TweenTo(
                    target: bullet,
                    expression: b => b.Velocity,
                    toValue: Vector2.Normalize(new Vector2(bullet.Velocity.X, bullet.Velocity.Y)) * VelocityLengthTrigger,
                    duration: duration, delay: 0)
                    .Easing(EasingFunctions.CubicIn);
            }
            tweener.Update(gameTime.GetElapsedSeconds());
            // Decrease the velocity
            //bullet.Velocity *= MathF.Pow(0.25f, gameTime.GetElapsedSeconds()); // Change the calculation as per your requirement

            if (bullet.Velocity.Length() <= VelocityLengthTrigger + epsilon)
            {
                foreach (var behavior in OnVelocityTriggerBehaviors)
                {
                    behavior.Execute(bullet);
                }
                bullet.Alive = false;
            }
        }

        public void Execute(Bullet bullet)
        {
            // Code to execute if this behavior is triggered by another behavior
        }

        public IBulletBehavior Clone()
        {
            DecreaseVelocityOverTimeBehavior cloned = new DecreaseVelocityOverTimeBehavior(duration,0);
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
