using FantaRPG.src.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tweening;
using MonoGame.Extended;

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
        private Tweener tweener;
        public FadeToBlack(bool reverse = false)
        {
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
            tweener = new Tweener();
            tweener.TweenTo(target: this,
                            expression: fade => Alpha,
                            toValue: alphaEnd,
                            duration: 5,
                            delay: 0)
                   .Easing(EasingFunctions.Linear);
        }
        public void Draw(SpriteBatch spriteBatch, Camera cam)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred);
            spriteBatch.Draw(Game1.Instance.pixel, new Rectangle(0, 0, cam.Bounds.Width, cam.Bounds.Height), col);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            tweener.Update(gameTime.GetElapsedSeconds());
            col.A = (byte)alpha;

        }
    }
}
