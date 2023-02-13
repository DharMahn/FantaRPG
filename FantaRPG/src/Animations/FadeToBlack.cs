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
        private Tweener tweener = new Tweener();
        private Tween tween;
        public bool IsReverse { get; }
        public FadeToBlack(bool reverse = false)
        {
            IsReverse = reverse;
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
            tween = tweener.TweenTo(target: this,
                            expression: fade => Alpha,
                            toValue: alphaEnd,
                            duration: 5f,
                            delay: 0)
                   .Easing(EasingFunctions.Linear);
        }
        public void Draw(SpriteBatch spriteBatch, Camera cam)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred);
            spriteBatch.Draw(Game1.Instance.pixel, new Rectangle(0, 0, cam.Bounds.Width, cam.Bounds.Height), col);
            spriteBatch.End();
        }
        public bool IsFinished => tween.IsComplete;
        public void Update(GameTime gameTime)
        {
            tweener.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            col.A = (byte)alpha;

        }
    }
}
