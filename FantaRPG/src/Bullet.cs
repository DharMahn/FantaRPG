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
using FantaRPG.src.Events;

namespace FantaRPG.src
{
    internal class Bullet : Entity
    {
        private static readonly FieldInfo ParticleEmitterInfo = typeof(ParticleEmitter).GetField("_random", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo FastRandomInfo = typeof(FastRandom).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance);

        bool gravityAffected = true;
        private ParticleEmitter emitter;
        public event EventHandler OnCollision;
        private float damage;
        public Bullet(float x, float y, Vector2 size, Vector2 velocity, float dmg, Texture2D texture = null) : base(x, y, size, texture)
        {
            damage = dmg;
            Velocity = velocity;
            TextureRegion2D textureRegion = new(Game1.Instance.pixel);
            emitter = new ParticleEmitter(textureRegion, 20, TimeSpan.FromSeconds(.5), Profile.Circle(20, Profile.CircleRadiation.Out))
            {
                AutoTrigger = false,
                Parameters = new ParticleReleaseParameters
                {
                    Speed = new Range<float>(0f, 500f),
                    //Scale = new Range<float>(0, w),
                    Quantity = 20,
                },
                Modifiers =
                {
                    new AgeModifier
                    {
                        Interpolators =
                        {
                            new ColorInterpolator
                            {
                                StartValue = new HslColor(0.0f,1.0f,0.5f),
                                EndValue = new HslColor(180.0f,1.0f,0.5f)
                            },
                            new ScaleInterpolator()
                            {
                                StartValue = new Vector2(hitboxSize.X,hitboxSize.Y),
                                EndValue = Vector2.Zero,
                            }
                        }
                    }
                }
            };
            OnCollision += delegate
            {
                Game1.Instance.CurrentRoom.AddEmitter(emitter);

                FastRandom random = (FastRandom)ParticleEmitterInfo.GetValue(emitter);
                FastRandomInfo.SetValue(random, RNG.Get(100000));
                emitter.Trigger(Bounds.Position);
                //FastRandom random = emitter.GetFieldValue<FastRandom>("_random");
                //int val = random.GetFieldValue<int>("_state");
                //Debug.WriteLine(val);
            };
        }
        Entity collidedEntity = null;
        public override void Update(GameTime gameTime)
        {
            if (alive)
            {
                if (gravityAffected)
                {
                    velocity.Y += Game1.Instance.CurrentRoom.Gravity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                foreach (var item in Game1.Instance.CurrentRoom.Objects)
                {
                    //if (IsTouchingLeft(item, gameTime))
                    //{
                    //    float tempPosX = position.X;
                    //    position.X = item.Bounds.Position.X - HitboxSize.X;
                    //    float ratio = Math.Abs(position.X - tempPosX) / velocity.X;
                    //    position.Y += (velocity.Y * ratio);
                    //    alive = false;
                    //    collidedEntity = item;
                    //}
                    //else if (IsTouchingRight(item, gameTime))
                    //{
                    //    float tempPosX = position.X;
                    //    position.X = item.Bounds.Position.X + item.HitboxSize.X;
                    //    float ratio = Math.Abs(position.X - tempPosX) / velocity.X;
                    //    position.Y -= (velocity.Y * ratio);
                    //    alive = false;
                    //    collidedEntity = item;
                    //}
                    //if (IsTouchingTop(item, gameTime))
                    //{
                    //    float tempPosY = position.Y;
                    //    position.Y = item.Bounds.Position.Y - HitboxSize.Y;
                    //    float ratio = Math.Abs(position.Y - tempPosY) / velocity.Y;
                    //    position.X += (velocity.X * ratio);
                    //    alive = false;
                    //    collidedEntity = item;
                    //}
                    //else if (IsTouchingBottom(item, gameTime))
                    //{
                    //    float tempPosY = position.Y;
                    //    position.Y = item.Bounds.Position.Y + item.HitboxSize.Y;
                    //    float ratio = Math.Abs(position.Y - tempPosY) / velocity.Y;
                    //    position.X -= (velocity.X * ratio);
                    //    alive = false;
                    //    collidedEntity = item;
                    //}
                }
                //base.Update(gameTime);
                if (alive)
                {
                    base.Update(gameTime);
                    //Position += Vector2.Multiply(Velocity, (float)gameTime.GetElapsedSeconds());
                }
                else
                {
                    collidedEntity?.Damage(damage);
                    OnCollision(this, null);
                }
            }
        }
    }
}
