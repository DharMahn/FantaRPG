using FantaRPG.src.Modifiers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FantaRPG.src
{
    [Serializable]
    internal class Bullet : Entity
    {
        protected static readonly FieldInfo ParticleEmitterInfo = typeof(ParticleEmitter).GetField("_random", BindingFlags.NonPublic | BindingFlags.Instance);
        protected static readonly FieldInfo FastRandomInfo = typeof(FastRandom).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance);
        protected static readonly float maxLifeTime = 5;
        protected float lifeTime = 0;
        private readonly ParticleEmitter emitter;
        public event EventHandler OnDeath;
        protected readonly List<IBulletBehavior> behaviors = [];
        protected readonly float damage;
        protected Vector2 OriginalVelocity;
        public float Damage => damage;
        protected Entity? owner;
        public Entity? Owner => owner;
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
                            new ScaleInterpolator()
                            {
                                StartValue = new Vector2(hitboxSize.X,hitboxSize.Y),
                                EndValue = Vector2.Zero,
                            }
                        }
                    }
                }
            };
            OnDeath += delegate
            {
                Game1.Instance.CurrentRoom.AddEmitter(emitter);

                FastRandom random = (FastRandom)ParticleEmitterInfo.GetValue(emitter);
                FastRandomInfo.SetValue(random, RNG.Get(100000));
                emitter.Trigger(Position);
                //new SplitBehavior(1).Execute(this);
                //FastRandom random = emitter.GetFieldValue<FastRandom>("_random");
                //int val = random.GetFieldValue<int>("_state");
                //Debug.WriteLine(val);
            };
        }

        public Bullet(Bullet bullet) : this(bullet.position.X, bullet.position.Y, new Vector2(bullet.HitboxSize.X, bullet.hitboxSize.Y), bullet.OriginalVelocity, bullet.damage, bullet.owner, bullet.Texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            lifeTime += gameTime.GetElapsedSeconds();
            if (lifeTime > maxLifeTime)
            {
                alive = false;
            }
            if (alive)
            {
                
                foreach (Platform item in Game1.Instance.CurrentRoom.Platforms)
                {
                    if (item.IsCollidable)
                    {
                        if (IsTouchingLeftOf(item, gameTime))
                        {
                            position.X = item.Position.X - HitboxSize.X;
                            velocity.X *= -1;
                            alive = false;
                        }
                        else if (IsTouchingRightOf(item, gameTime))
                        {
                            position.X = item.Position.X + item.HitboxSize.X;
                            velocity.X *= -1;
                            alive = false;
                        }
                        if (IsTouchingTopOf(item, gameTime))
                        {
                            position.Y = item.Position.Y - HitboxSize.Y;
                            velocity.Y *= -1;
                            alive = false;
                        }
                        else if (IsTouchingBottomOf(item, gameTime))
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
                    foreach (IBulletBehavior behavior in behaviors)
                    {
                        behavior.Update(this, gameTime);
                    }
                }
                else
                {
                    DoDeath();
                }
            }
            else
            {
                DoDeath();
            }
        }
        private void DoDeath()
        {
            OnDeath(this, null);
        }
        public void AddBehavior(IBulletBehavior behavior)
        {
            behaviors.Add(behavior);
        }

        public void CopyBehaviorsFrom(Bullet bullet)
        {
            foreach (IBulletBehavior item in bullet.behaviors)
            {
                if (item.PassCount > 0)
                {
                    behaviors.Add(item.Clone());
                }
            }
        }
        public void CopyAllBehaviorsFrom(Bullet bullet)
        {
            foreach (IBulletBehavior item in bullet.behaviors)
            {
                behaviors.Add(item.Clone());
            }
        }
    }
}
