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
using System.Reflection;

namespace FantaRPG.src
{
    internal class Bullet : Entity
    {
        private static FieldInfo ParticleEmitterInfo = typeof(ParticleEmitter).GetField("_random", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo FastRandomInfo = typeof(FastRandom).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance);

        bool gravityAffected = true;
        private ParticleEmitter emitter;
        List<Action> OnCollideAction = new List<Action>();
        public void AddOnCollisionAction(Action action)
        {
            OnCollideAction.Add(action);
        }
        public Bullet(Texture2D texture, int x, int y, int w, int h, Vector2 velocity) : base(texture, x, y, w, h)
        {
            Velocity = velocity;
            TextureRegion2D textureRegion = new TextureRegion2D(texture);
            emitter = new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(.5), Profile.Circle(20, Profile.CircleRadiation.Out))
            {
                AutoTrigger = false,
                Parameters = new ParticleReleaseParameters
                {
                    Speed = new Range<float>(0f, 500f),
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

                FastRandom random = (FastRandom)ParticleEmitterInfo.GetValue(emitter);
                FastRandomInfo.SetValue(random, RNG.Get(100000));
                emitter.Trigger(Position);
                //FastRandom random = emitter.GetFieldValue<FastRandom>("_random");
                //int val = random.GetFieldValue<int>("_state");
                //Debug.WriteLine(val);
            });
        }
        public override void Update(GameTime gameTime)
        {
            if (alive)
            {
                if (gravityAffected)
                {
                    Velocity.Y += Game1.Instance.CurrentRoom.Gravity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
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
                else
                {
                    foreach (var item in OnCollideAction)
                    {
                        item.Invoke();
                    }
                }
            }
        }
    }
}
