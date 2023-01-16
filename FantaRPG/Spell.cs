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

namespace FantaRPG
{
    internal class Spell : Entity
    {
        bool gravityAffected = true;
        private ParticleEffect emitter;
        bool alive = true;
        bool triggered = false;
        public bool Finished = false;
        public Spell(Texture2D texture, int x, int y, int w, int h, Vector2 velocity) : base(texture, x, y, w, h)
        {
            Velocity = velocity;
            TextureRegion2D textureRegion=new TextureRegion2D(texture);
            emitter = new ParticleEffect(autoTrigger: false)
            {
                Position = Vector2.Zero,
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 50, TimeSpan.FromSeconds(.5), Profile.Circle(10, Profile.CircleRadiation.Out))
                    {
                        AutoTrigger=false,
                        //AutoTrigger=false,
                        //AutoTriggerFrequency=0,
                        Parameters=new ParticleReleaseParameters
                        {
                            Speed=new Range<float>(100f,300f),
                            Scale=new Range<float>(w),
                            Quantity=50,
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
                    }
                }
            };
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
                        Velocity.X *= -1;
                        alive = false;
                    }
                    else if (IsTouchingRight(item, gameTime))
                    {
                        Position.X = item.Position.X + item.HitboxSize.X;
                        Velocity.X *= -1;
                        alive = false;
                    }
                    if (IsTouchingTop(item, gameTime))
                    {
                        Position.Y = item.Position.Y - HitboxSize.Y;
                        Velocity.Y *= -1;
                        alive = false;
                    }
                    else if (IsTouchingBottom(item, gameTime))
                    {
                        Position.Y = item.Position.Y + item.HitboxSize.Y;
                        Velocity.Y *= -1;
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
                    emitter.Position= Position + Vector2.Multiply(HitboxSize, 0.5f);
                    emitter.Trigger(Position+Vector2.Multiply(HitboxSize,0.5f));
                    emitter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                    triggered = true;
                }
                else
                {
                    //Debug.WriteLine(emitter.ActiveParticles + " + " + emitter.Emitters[0].AutoTrigger);
                    Finished = !emitter.Emitters[0].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(emitter);
            if (alive)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
