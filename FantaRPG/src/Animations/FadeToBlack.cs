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

namespace FantaRPG.src.Animations
{
    internal class FadeToBlack : IAnimation
    {
        private Color col = Color.Black;
        private float alpha;
        private float alphaEnd;
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }
        //private Tweener tweener = new Tweener();
        //private Tween tween;
        public bool IsReverse { get; }
        float interval;
        public FadeToBlack(float interval, bool reverse = false)
        {
            IsReverse = reverse;
            this.interval = interval;
            if (reverse)
            {
                alpha = 255;
                alphaEnd = 0;
            }
            else
            {
                alpha = 0;
                alphaEnd = 255;
            }
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
                next = alpha + (1f * ((float)gameTime.ElapsedGameTime.TotalSeconds / interval));
            }
            else
            {
                next = alpha - (1f * ((float)gameTime.ElapsedGameTime.TotalSeconds / interval));
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
            //tweener.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            col.A = (byte)alpha;
        }
    }
}
