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
        private Room targetRoom = null;
        private bool isTriggered = false;
        public Portal(Texture2D texture, float x, float y, float w, float h) : base(texture, x, y, w, h)
        {
            IsCollidable = false;
        }
        public Portal(Texture2D texture, float x, float y, float w, float h, Room room) : base(texture, x, y, w, h)
        {
            SetAsPortal(room);
            IsCollidable = false;
        }
        public void SetAsPortal(Room target)
        {
            targetRoom = target;
        }
        public void ChangeRoom()
        {
            if (isTriggered) return;
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
