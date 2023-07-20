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
        private static readonly Vector3 InactiveColor = Color.Red.ToVector3();
        private static readonly Vector3 ActiveColor = Color.Blue.ToVector3();
        private Vector3 currentColor;
        public Vector3 CurrentColor { get { return currentColor; } set { currentColor = value; } }
        float rotation = 0f;
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public bool IsWorking
        {
            get
            {
                return isWorking;
            }
            set
            {
                isWorking = value;
                if (value)
                {
                    disableTime = 0;
                }
                else
                {
                    disableTime = maxDisableTime;
                }
            }
        }

        public Portal TargetPortal { get { return targetPortal; } }
        public Room ContainingRoom { get { return containingRoom; } }
        static float maxDisableTime = 0.75f;
        float disableTime = maxDisableTime;
        Tweener tweener = new Tweener();
        public Portal(Room parent, float x, float y, Vector2 size) : base(x, y, size)
        {
            IsCollidable = false;
            containingRoom = parent;
            IsWorking = true;
            CurrentColor = isWorking ? ActiveColor : InactiveColor;
            Texture = Game1.Instance.Content.Load<Texture2D>("portal0");
            //FadeOutNow();
            //FadeIn();
        }
        public Portal(Room parent, float x, float y, Vector2 size, Portal targetPortal) : this(parent, x, y, size)
        {
            SetPortalTo(targetPortal);
        }
        public void SetPortalTo(Portal target)
        {
            targetPortal = target;
            IsWorking = false;
        }
        public void SetPortalTo(Room room)
        {
            if (!room.HasPortalTo(containingRoom))
            {
                targetPortal = room.SetRandomPortalTo(this);
                IsWorking = false;
            }
        }
        public void FadeOutNow()
        {
            Vector2 tempCenter = new Vector2(position.X + (hitboxSize.X / 2), position.Y + (hitboxSize.Y / 2));
            hitboxSize = Vector2.Zero;
            Center = tempCenter;
            disableTime = maxDisableTime;
            currentColor = InactiveColor;
        }
        public void FadeInNow()
        {
            Vector2 tempCenter = new Vector2(Center.X, Center.Y);
            hitboxSize = EntityConstants.PortalSize;
            Center = tempCenter;
            currentColor = ActiveColor;
        }
        public bool FadeIn()
        {

            Vector2 initialCenter = this.Center;
            tweener.TweenTo(this, x => x.HitboxSize, EntityConstants.PortalSize, 0.5f, maxDisableTime - 0.75f).Easing(EasingFunctions.ElasticOut);
            tweener.TweenTo(this, x => x.Center, initialCenter, 0.5f, maxDisableTime - 0.75f).Easing(EasingFunctions.ElasticOut);
            tweener.TweenTo(this, x => x.CurrentColor, ActiveColor, 0.5f, maxDisableTime - 0.75f).Easing(EasingFunctions.SineOut);
            tweener.TweenTo(this, x => x.Rotation, rotation - 6, 0.5f, maxDisableTime - 0.75f).Easing(EasingFunctions.SineOut);

            return true;
        }
        public bool FadeOut()
        {

            Vector2 initialCenter = this.Center;
            tweener.TweenTo(this, x => x.HitboxSize, Vector2.Zero, 0.5f, maxDisableTime - 0.75f).Easing(EasingFunctions.ElasticOut);
            tweener.TweenTo(this, x => x.Center, initialCenter, 0.5f, maxDisableTime - 0.75f).Easing(EasingFunctions.ElasticOut);
            tweener.TweenTo(this, x => x.CurrentColor, InactiveColor, 0.5f, maxDisableTime - 0.75f).Easing(EasingFunctions.SineOut);
            tweener.TweenTo(this, x => x.Rotation, rotation - 6, 0.5f, maxDisableTime - 0.75f).Easing(EasingFunctions.SineOut);


            return true;
        }
        public void ChangeRoom()
        {
            if (!IsWorking) return;
            if (!targetPortal.containingRoom.HasPortalTo(this))
            {
                targetPortal.containingRoom.SetRandomPortalTo(this);
            }
            Game1.Instance.UsePortal(this);
            IsWorking = false;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            tweener.Update(gameTime.GetElapsedSeconds());
            if (disableTime > 0)
            {
                disableTime -= gameTime.GetElapsedSeconds();
            }
            if (disableTime <= 0)
            {
                disableTime = 0;
                IsWorking = true;
            }
            rotation -= gameTime.GetElapsedSeconds() * .5f;
            Debug.WriteLine(position.ToString());
        }
        public void Reset()
        {
            IsWorking = true;
            disableTime = maxDisableTime;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle sourceRectangle = new Rectangle(0, 0, (int)HitboxSize.X, (int)HitboxSize.Y);
            //Rectangle destinationRectangle = new Rectangle(
            //    (int)(Position.X + (1/((EntityConstants.PortalSize.X / 2) * disableTime))),
            //    (int)(Position.Y + (1/((EntityConstants.PortalSize.X / 2) * disableTime))),
            //    Math.Max(0, (int)(HitboxSize.X - (1/((EntityConstants.PortalSize.X / 2) * disableTime)))),
            //    Math.Max(0, (int)(HitboxSize.Y - (1/((EntityConstants.PortalSize.X / 2) * disableTime)))));
            Rectangle destinationRectangle = new Rectangle((int)(Position.X-(hitboxSize.X/2f)), (int)(Position.Y - (hitboxSize.Y / 2f)), (int)HitboxSize.X, (int)HitboxSize.Y);
            //spriteBatch.DrawRectangle(destinationRectangle, Color.Blue);
            destinationRectangle.X += (int)hitboxSize.X;
            destinationRectangle.Y += (int)hitboxSize.Y;
            spriteBatch.Draw(Texture, destinationRectangle, null, new Color(currentColor.X, currentColor.Y, currentColor.Z), rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
