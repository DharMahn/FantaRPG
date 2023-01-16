using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;
using System.Diagnostics;

namespace FantaRPG.src
{
    internal class Bullet : Entity
    {
        bool gravityAffected = true;
        private ParticleEmitter emitter;
        bool triggered = false;
        public bool Finished = false;
        List<Action> OnCollideAction = new List<Action>();
        public void AddOnCollisionAction(Action action)
        {
            OnCollideAction.Add(action);
        }
        public Bullet(Texture2D texture, int x, int y, int w, int h, Vector2 velocity) : base(texture, x, y, w, h)
        {
            Velocity = velocity;
            TextureRegion2D textureRegion = new TextureRegion2D(texture);
            emitter = new ParticleEmitter(textureRegion, 50, TimeSpan.FromSeconds(.5), Profile.Circle(10, Profile.CircleRadiation.Out))
            {
                AutoTrigger = false,
                //AutoTrigger=false,
                //AutoTriggerFrequency=0,
                Parameters = new ParticleReleaseParameters
                {
                    Speed = new Range<float>(100f, 300f),
                    Scale = new Range<float>(w),
                    Quantity = 50,
                },
                Modifiers =
                    {
                        new AgeModifier
                        {
                            Interpolators =
                            {
                                new ColorInterpolator
                                {
                                    StartValue=new HslColor(0.0f,1.0f,0.5f),
                                    EndValue=new HslColor(180.0f,1.0f,0.5f)
                                },
                                new ScaleInterpolator()
                                {
                                    StartValue=new Vector2(w,w),
                                    EndValue=Vector2.Zero,
                                }
                            }
                        }
                    }
                };
            AddOnCollisionAction(() =>
            {
                Game1.Instance.CurrentRoom.AddEmitter(emitter);
                emitter.Trigger(Position);
            });
        }
        public new void Update(GameTime gameTime)
        {
            if (alive)
            {
                if (gravityAffected)
                {
                    Velocity.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                foreach (var item in Game1.Instance.CurrentRoom.Platforms)
                {
                    if (IsTouchingLeft(item, gameTime))
                    {
                        Position.X = item.Position.X - HitboxSize.X;
                        alive = false;
                    }
                    else if (IsTouchingRight(item, gameTime))
                    {
                        Position.X = item.Position.X + item.HitboxSize.X;
                        alive = false;
                    }
                    if (IsTouchingTop(item, gameTime))
                    {
                        Position.Y = item.Position.Y - HitboxSize.Y;
                        alive = false;
                    }
                    else if (IsTouchingBottom(item, gameTime))
                    {
                        Position.Y = item.Position.Y + item.HitboxSize.Y;
                        alive = false;
                    }
                }
                if (alive)
                {
                    Position += Vector2.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
            else
            {
                if (!triggered)
                {
                    //emitter.Trigger(Position + Vector2.Multiply(HitboxSize, 0.5f));
                    //emitter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                    triggered = true;
                    foreach (var item in OnCollideAction)
                    {
                        item.Invoke();
                    }
                }
                else
                {
                    //Debug.WriteLine(emitter.ActiveParticles + " + " + emitter.Emitters[0].AutoTrigger);
                    //Finished = !emitter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
