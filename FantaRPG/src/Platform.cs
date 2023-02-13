using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FantaRPG.src.Interfaces;

namespace FantaRPG.src
{
    internal class Platform : Entity
    {
        public bool IsCollidable = true;
        public bool IsDoor { get; private set; }
        private Room targetRoom = null;
        private bool isTriggered = false;
        public void SetAsDoor(Room target)
        {
            targetRoom = target;
            IsDoor = true;
        }

        public void ChangeRoom()
        {
            if (isTriggered) return;
            Game1.Instance.TransitionToRoom(targetRoom);
            isTriggered = true;
        }

        public Platform(Texture2D texture, int x, int y, int w, int h) : base(texture, x, y, w, h)
        {
            IsDoor = false;
        }
        public Platform(Texture2D texture, int x, int y, int w, int h, Room target) : base(texture, x, y, w, h)
        {
            SetAsDoor(target);
        }
    }
}
