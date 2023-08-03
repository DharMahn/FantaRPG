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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FantaRPG.src.Modifiers;
using Newtonsoft.Json.Linq;

namespace FantaRPG.src
{
    [Serializable]
    internal class Bullet : Entity
    {
        private static readonly FieldInfo ParticleEmitterInfo = typeof(ParticleEmitter).GetField("_random", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo FastRandomInfo = typeof(FastRandom).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance);

        bool gravityAffected = false;
        private ParticleEmitter emitter;
        public event EventHandler OnCollision;
        private List<IBulletBehavior> behaviors = new List<IBulletBehavior>();
        private float damage;
        private Vector2 OriginalVelocity;
        public float Damage { get { return damage; } }
        Entity owner;
        public Entity Owner { get { return owner; } }
        public Bullet(float x, float y, Vector2 size, Vector2 velocity, float dmg, Entity? owner, Texture2D texture = null) : base(x, y, size, texture)
        {
            damage = dmg;
            Velocity = velocity;
            OriginalVelocity = velocity;
            TextureRegion2D textureRegion = new(Game1.Instance.pixel);
            this.owner = owner;
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
                emitter.Trigger(Position);
                //FastRandom random = emitter.GetFieldValue<FastRandom>("_random");
                //int val = random.GetFieldValue<int>("_state");
                //Debug.WriteLine(val);
            };
        }

        public Bullet(Bullet bullet)
        {
            position = bullet.position;
            hitboxSize = bullet.hitboxSize;
            Velocity = bullet.OriginalVelocity;
            OriginalVelocity = bullet.OriginalVelocity;
            
            damage = bullet.damage;
            owner = bullet.owner;
            Texture = bullet.Texture;
        }

        public override void Update(GameTime gameTime)
        {
            if (alive)
            {
                if (gravityAffected)
                {
                    velocity.Y += Game1.Instance.CurrentRoom.Gravity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                foreach (var item in Game1.Instance.CurrentRoom.Platforms)
                {
                    if (item.IsCollidable)
                    {
                        if (IsTouchingLeft(item, gameTime))
                        {
                            position.X = item.Position.X - HitboxSize.X;
                            velocity.X *= -1;
                            alive = false;
                        }
                        else if (IsTouchingRight(item, gameTime))
                        {
                            position.X = item.Position.X + item.HitboxSize.X;
                            velocity.X *= -1;
                            alive = false;
                        }
                        if (IsTouchingTop(item, gameTime))
                        {
                            position.Y = item.Position.Y - HitboxSize.Y;
                            velocity.Y *= -1;
                            alive = false;
                        }
                        else if (IsTouchingBottom(item, gameTime))
                        {
                            position.Y = item.Position.Y + item.HitboxSize.Y;
                            alive = false;
                            velocity.Y *= -1;
                        }
                    }
                }
                //base.Update(gameTime);
                if (alive)
                {
                    base.Update(gameTime);
                    //Position += Vector2.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);
                    foreach (var behavior in behaviors)
                    {
                        behavior.Update(this, gameTime);
                    }
                }
                else
                {

                    OnCollision(this, null);
                }
            }
        }
        public void AddBehavior(IBulletBehavior behavior)
        {
            behaviors.Add(behavior);
        }

        internal void CopyBehaviorsFrom(Bullet bullet)
        {
            behaviors = bullet.behaviors;
        }
    }
}
