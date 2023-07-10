using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class Portal : Platform
    {
        private Room parentRoom=null;
        private Room targetRoom = null;
        private bool isTriggered = false;
        public Room TargetRoom { get { return targetRoom; } }
        public Room ParentRoom { get { return parentRoom; } }

        public Portal(Room parent, float x, float y, Vector2 size) : base(x, y, size)
        {
            IsCollidable = false;
            parentRoom = parent;
        }
        public Portal(Room parent, float x, float y, Vector2 size, Room targetRoom) : this(parent, x, y, size)
        {
            SetPortalTo(targetRoom);
        }
        public void SetPortalTo(Room target)
        {
            targetRoom = target;
        }
        public void ChangeRoom()
        {
            if (isTriggered) return;
            if (!targetRoom.HasPortalTo(parentRoom))
            {
                targetRoom.SetRandomPortalTo(parentRoom);
            }
            Game1.Instance.TransitionToRoom(targetRoom);
            isTriggered = true;
        }
        public void Reset()
        {
            isTriggered = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y), Color.Purple);
        }
    }
}
