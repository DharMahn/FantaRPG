using FantaRPG.src.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FantaRPG.src.Animations
{
    internal class FadeToBlack : IAnimation
    {
        private Color col = Color.Black;

        public float Alpha { get; set; }
        //private Tweener tweener = new Tweener();
        //private Tween tween;
        public bool IsReverse { get; }

        private readonly float interval;
        public FadeToBlack(bool reverse = false, float interval = .07f)
        {
            IsReverse = reverse;
            this.interval = interval;
            Alpha = reverse ? 255 : 0;
            col.A = (byte)Alpha;
        }
        public void Draw(SpriteBatch spriteBatch, Camera cam)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred);
            spriteBatch.Draw(Game1.Instance.pixel, new Rectangle(0, 0, cam.Bounds.Width, cam.Bounds.Height), col);
            spriteBatch.End();
        }
        public bool IsFinished { get; private set; }
        public void Update(GameTime gameTime)
        {
            float next = IsReverse
                ? Alpha - (255f * ((float)gameTime.ElapsedGameTime.TotalSeconds / interval))
                : Alpha + (255f * ((float)gameTime.ElapsedGameTime.TotalSeconds / interval));
            if (next >= 255)
            {
                next = 255;
                IsFinished = true;
            }
            else if (next <= 0)
            {
                next = 0;
                IsFinished = true;
            }
            Alpha = next;
            //tweener.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            col.A = (byte)Alpha;
        }
    }
}
