using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace FantaRPG.src
{
    internal class BasicCollision : ICollisionActor
    {
        protected Vector2 position, velocity, hitboxSize;
        public Vector2 Velocity { get { return velocity; } protected set { velocity = value; } }
        public Vector2 HitboxSize => ((RectangleF)Bounds).Size;

        public IShapeF Bounds { get; protected set; }

        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {
            Bounds.Position -= collisionInfo.PenetrationVector;
            if (collisionInfo.PenetrationVector.X != 0)
            {
                velocity.X = 0;
            }
            if (collisionInfo.PenetrationVector.Y != 0)
            {
                velocity.Y = 0;
            }
        }
    }
}