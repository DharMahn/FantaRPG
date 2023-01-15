using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG
{
    internal class Spell : Entity
    {
        bool gravityAffected = false;
        public Spell(Texture2D texture, int x, int y, int w, int h, Vector2 velocity) : base(texture, x, y, w, h)
        {
            Velocity = velocity;
        }
        public new void Update(GameTime gameTime)
        {
            if (gravityAffected)
            {
                Velocity.Y+=100*(float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            foreach (var item in Game1.Instance.CurrentRoom.Platforms)
            {
                if (IsTouchingLeft(item, gameTime))
                {
                    Position.X = item.Position.X - HitboxSize.X;
                    Velocity.X *= -1;
                }
                else if (IsTouchingRight(item, gameTime))
                {
                    Position.X = item.Position.X + item.HitboxSize.X;
                    Velocity.X *= -1;
                }
                if (IsTouchingTop(item, gameTime))
                {
                    Position.Y = item.Position.Y - HitboxSize.Y;
                    Velocity.Y *= -1;
                }
                else if (IsTouchingBottom(item, gameTime))
                {
                    Position.Y = item.Position.Y + item.HitboxSize.Y;
                    Velocity.Y *= -1;
                }
            }
            Position += Vector2.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
