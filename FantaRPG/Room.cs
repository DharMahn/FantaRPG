using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG
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
        public bool AddEntity(Entity entity)
        {
            entities.Add(entity);
            return true;
        }
        public Room(List<BackgroundLayer> bgs)
        {
            backgrounds = bgs;
            platforms = new List<Platform>();
        }
        public Room(List<BackgroundLayer> bgs, List<Platform> platforms, List<Entity> entities, Player player)
        {
            backgrounds = bgs;
            this.platforms = platforms;
            this.entities= entities;
            Player=player;
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
        internal void DrawEntities(SpriteBatch spriteBatch, Matrix matrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred,blendState: BlendState.AlphaBlend,transformMatrix: matrix);
            foreach (var item in entities)
            {
                if (item is Spell)
                {
                    (item as Spell).Draw(spriteBatch);
                }
                else
                {
                    item.Draw(spriteBatch);
                }
            }
            player.Draw(spriteBatch);
            spriteBatch.End();
        }
        internal void DrawPlatforms(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred,transformMatrix: transform);
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
            foreach (var item in entities)
            {
                if (item is Spell)
                {
                    (item as Spell).Update(gameTime);
                }
            }
            player.Update(gameTime);
        }
    }
}
