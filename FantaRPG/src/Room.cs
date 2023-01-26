using FantaRPG.src.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class Room
    {
        private List<BackgroundLayer> backgrounds;
        public List<BackgroundLayer> Backgrounds
        {
            get { return backgrounds; }
        }
        private List<IEntity> entities;
        public List<IEntity> Entities
        {
            get { return entities; }
        }
        ParticleEffect particles;
        public void AddEmitter(ParticleEmitter effect)
        {
            particles.Emitters.Add(effect);
        }
        public Rectangle Bounds { get; protected set; }
        public bool AddEntity(IEntity entity)
        {
            entities.Add(entity);
            return true;
        }
        public Room(List<BackgroundLayer> bgs, List<IEntity> entities, Player player, Rectangle bounds = new Rectangle())
        {
            backgrounds = bgs;
            this.entities = entities;
            particles = new ParticleEffect(autoTrigger: false)
            {
                Position = Vector2.Zero
            };
            Player = player;
            Bounds = bounds;
        }
        private Player player;

        public Player Player
        {
            get { return player; }
            private set { player = value; }
        }

        internal void DrawBackground(SpriteBatch spriteBatch, Camera cam)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);
            foreach (var item in Backgrounds)
            {
                item.Draw(spriteBatch, cam);
            }
            spriteBatch.End();
        }
        internal void DrawEntities(SpriteBatch spriteBatch, Camera cam)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, transformMatrix: cam.Transform);
            foreach (var item in entities)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.Draw(particles);
            player.Draw(spriteBatch);
            spriteBatch.End();
        }

        internal void Update(GameTime gameTime)
        {
            foreach (var item in entities.ToList())
            {
                if (item is Bullet)
                {
                    Bullet bullet = item as Bullet;
                    if (!bullet.Alive)
                    {
                        entities.Remove(bullet);
                        continue;
                    }
                    (item as Bullet).Update(gameTime);
                }
            }
            foreach (var item in particles.Emitters.ToList())
            {
                Console.WriteLine("asd");
                if (!item.Update((float)gameTime.ElapsedGameTime.TotalSeconds))
                {
                    particles.Emitters.Remove(item);
                }
            }
            player.Update(gameTime);
        }
    }
}
