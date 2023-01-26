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
using MonoGame.Extended.Collisions;
using FantaRPG.src.Interfaces;

namespace FantaRPG.src
{
    internal class Bullet : IEntity
    {
        public IShapeF Bounds { get; }
        private Texture2D Texture;
        public Vector2 Velocity;
        private static FieldInfo ParticleEmitterInfo = typeof(ParticleEmitter).GetField("_random", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo FastRandomInfo = typeof(FastRandom).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance);
        private bool alive;

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        bool gravityAffected = true;
        private ParticleEmitter emitter;
        List<Action> OnCollideAction = new List<Action>();
        public void AddOnCollisionAction(Action action)
        {
            OnCollideAction.Add(action);
        }
        public Bullet(Texture2D texture, IShapeF shape, Vector2 velocity)
        {
            Velocity = velocity;
            TextureRegion2D textureRegion = new TextureRegion2D(texture);
            emitter = new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(.5), Profile.Circle(20, Profile.CircleRadiation.Out))
            {
                AutoTrigger = false,
                Parameters = new ParticleReleaseParameters
                {
                    Speed = new Range<float>(0f, 500f),
                    Scale = new Range<float>(0, 10),
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
                                StartValue = new HslColor(0.0f,1.0f,0.5f),
                                EndValue = new HslColor(180.0f,1.0f,0.5f)
                            },
                            new ScaleInterpolator()
                            {
                                StartValue = new Vector2(10,10),
                                EndValue = Vector2.Zero,
                            }
                        }
                    }
                }
            };
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            Game1.Instance.CurrentRoom.AddEmitter(emitter);

            FastRandom random = (FastRandom)ParticleEmitterInfo.GetValue(emitter);
            FastRandomInfo.SetValue(random, RNG.Get(100000));
            emitter.Trigger(Bounds.Position);
        }
        public void Update(GameTime gameTime)
        {
            if (gravityAffected)
            {
                Velocity.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            Bounds.Position += Vector2.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            RectangleF rectangle = (RectangleF)Bounds;
            spriteBatch.Draw(Texture, Bounds.Position, new Rectangle((int)rectangle.X,(int)rectangle.Y,(int)rectangle.Width,(int)rectangle.Height), Color.White);
        }
    }
}
