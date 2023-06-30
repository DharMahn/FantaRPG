using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantaRPG.src.Interfaces;
using MonoGame.Extended;

namespace FantaRPG.src.Animations
{
    internal class FadeToBlack : IAnimation
    {
        private Color col = Color.Black;
        private float alpha;
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }
        //private Tweener tweener = new Tweener();
        //private Tween tween;
        public bool IsReverse { get; }
        float interval;
        public FadeToBlack(bool reverse = false, float interval = .07f)
        {
            IsReverse = reverse;
            this.interval = interval;
            if (reverse)
            {
                alpha = 255;
            }
            else
            {
                alpha = 0;
            }
            col.A = (byte)alpha;
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
            float next;
            if (IsReverse)
            {
                next = alpha - (255f * ((float)gameTime.GetElapsedSeconds() / interval));
            }
            else
            {
                next = alpha + (255f * ((float)gameTime.GetElapsedSeconds() / interval));
            }
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
            alpha = next;
            //tweener.Update((float)gameTime.GetElapsedSeconds());
            col.A = (byte)alpha;
        }
    }
}
