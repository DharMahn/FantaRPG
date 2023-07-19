using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tweening;
using MonoGame.Extended;

namespace FantaRPG.src
{
    internal class Portal : Platform
    {
        private Room containingRoom = null;
        private Portal targetPortal = null;
        private bool isWorking = true;
        private bool IsWorking
        {
            get
            {
                return isWorking;
            }
            set
            {
                if (!value)
                {
                    tweener.TweenTo(this, x => x.HitboxSize, EntityConstants.PortalSize, (float)maxDisableTime, 0);
                }
                else
                {
                    tweener.TweenTo(this, x => x.HitboxSize, Vector2.Zero, (float)maxDisableTime, 0);

                }
            }
        }
        public Portal TargetPortal { get { return targetPortal; } }
        public Room ContainingRoom { get { return containingRoom; } }
        static double maxDisableTime = 1;
        double disableTime = maxDisableTime;
        Tweener tweener = new Tweener();
        public Portal(Room parent, float x, float y, Vector2 size) : base(x, y, size)
        {
            IsCollidable = false;
            containingRoom = parent;
            isWorking = true;
        }
        public Portal(Room parent, float x, float y, Vector2 size, Portal targetPortal) : this(parent, x, y, size)
        {
            SetPortalTo(targetPortal);
        }
        public void SetPortalTo(Portal target)
        {
            targetPortal = target;
            isWorking = false;
        }
        public void SetPortalTo(Room room)
        {
            if (!room.HasPortalTo(containingRoom))
            {
                targetPortal = room.SetRandomPortalTo(this);
                isWorking = false;
            }
        }
        public void ChangeRoom()
        {
            if (!isWorking) return;
            if (!targetPortal.containingRoom.HasPortalTo(this))
            {
                targetPortal.containingRoom.SetRandomPortalTo(this);
            }
            Game1.Instance.UsePortal(this);
            isWorking = false;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            tweener.Update(gameTime.GetElapsedSeconds());
            if (!isWorking)
            {
                disableTime -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (disableTime < 0)
            {
                disableTime = maxDisableTime;
                isWorking = true;
            }
        }
        public void Reset()
        {
            isWorking = true;
            disableTime = maxDisableTime;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y), isWorking ? Color.Purple : Color.Red);
        }
    }
}
