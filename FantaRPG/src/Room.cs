﻿using Microsoft.Xna.Framework;
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
        private List<Platform> platforms;
        public List<Platform> Platforms
        {
            get { return platforms; }
        }
        private List<Entity> entities;
        public List<Entity> Entities
        {
            get { return entities; }
        }
        ParticleEffect particles;
        public void AddEmitter(ParticleEmitter effect)
        {
            particles.Emitters.Add(effect);
        }
        public bool AddEntity(Entity entity)
        {
            entities.Add(entity);
            return true;
        }
        public Room(List<BackgroundLayer> bgs, List<Platform> platforms, List<Entity> entities, Player player)
        {
            backgrounds = bgs;
            this.platforms = platforms;
            this.entities = entities;
            particles = new ParticleEffect(autoTrigger: false)
            {
                Position = Vector2.Zero
            };
            Player = player;
        }
        private Player player;

        public Player Player
        {
            get { return player; }
            private set { player = value; }
        }

        internal void DrawBackground(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);
            foreach (var item in Backgrounds)
            {
                item.Draw(spriteBatch, transform);
            }
            spriteBatch.End();
        }
        internal void DrawEntities(SpriteBatch spriteBatch, Matrix matrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, transformMatrix: matrix);
            foreach (var item in entities)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.Draw(particles);
            player.Draw(spriteBatch);
            spriteBatch.End();
        }
        internal void DrawPlatforms(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, transformMatrix: transform);
            foreach (var item in platforms)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        public bool AddPlatform(Platform platform)
        {
            platforms.Add(platform);
            return true;
        }

        internal void Update(GameTime gameTime)
        {
            foreach (var item in entities.ToList())
            {
                if (item is Bullet)
                {
                    Bullet spell = item as Bullet;
                    if (spell.Finished)
                    {
                        entities.Remove(spell);
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