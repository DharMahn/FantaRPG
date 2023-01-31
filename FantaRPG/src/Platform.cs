using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FantaRPG.src.Interfaces;
using MonoGame.Extended.Collisions;
using MonoGame.Extended;
using MonoGame.Extended.Gui.Controls;

namespace FantaRPG.src
{
    internal class Platform : IEntity
    {
        public IShapeF Bounds { get; }
        private Texture2D Texture;
        public Vector2 Velocity;
        public bool IsCollidable = true;
        public bool IsDoor { get; private set; }
        public Room targetRoom = null;
        public void SetAsDoor(Room target)
        {
            targetRoom = target;
            IsDoor = true;
        }
        public Platform(Texture2D texture, RectangleF shape)
        {
            IsDoor = false;
            Texture = texture;
            Bounds = shape;
        }
        public Platform(Texture2D texture, RectangleF shape, Room target) : this(texture, shape)
        {
            SetAsDoor(target);
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (IsDoor)
            {
                Game1.Instance.ChangeRoom(targetRoom);
            }
        }

        public void Update(GameTime gameTime)
        {
            Bounds.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            RectangleF hitbox = (RectangleF)Bounds;
            spriteBatch.Draw(Texture, Bounds.Position, new Rectangle((int)hitbox.X, (int)hitbox.Y, (int)hitbox.Width, (int)hitbox.Height), Color.White);
        }
    }
}
