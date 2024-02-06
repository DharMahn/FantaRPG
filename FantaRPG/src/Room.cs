using FantaRPG.src.Pathfinding;
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
        public static List<(Room Room, bool IsValid)> Rooms = new();
        private readonly List<BackgroundLayer> backgrounds;
        public List<BackgroundLayer> Backgrounds
        {
            get { return backgrounds; }
        }
        private readonly List<Platform> platforms;
        public List<Platform> Platforms
        {
            get { return platforms; }
        }
        private readonly List<Entity> entities;
        public List<Entity> Entities
        {
            get { return entities; }
        }
        readonly ParticleEffect particles;
        private readonly List<Portal> portals;
        public List<Portal> Portals
        {
            get { return portals; }
        }
        public float Gravity { get; private set; }
        public void AddEmitter(ParticleEmitter effect)
        {
            particles.Emitters.Add(effect);
        }
        public Point Bounds { get; protected set; }
        public bool AddEntity(Entity entity)
        {
            entities.Add(entity);
            return true;
        }
        public static Room GetRandomValidRoom()
        {
            var validRooms = Rooms.Where(x => x.IsValid).ToList();
            var selectedRoom = validRooms[RNG.Get(validRooms.Count())];
            selectedRoom.IsValid = false;
            return selectedRoom.Room;
        }
        public static void ResetRoomValidities()
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                var room = Rooms[i].Room;
                Rooms[i] = (room, false);
            }
        }
        public Room(List<BackgroundLayer> bgs, List<Platform> platforms, List<Entity> entities, List<Portal> portals, Player player, Point bounds = new Point(), float gravity = 1f)
        {
            backgrounds = bgs;
            this.platforms = platforms;
            this.entities = entities;
            this.portals = portals;
            this.Gravity = gravity;
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

        internal void DrawBackground(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);
            foreach (var item in Backgrounds)
            {
                item.Draw(spriteBatch, transform);
            }
            spriteBatch.End();
        }
        public bool AddPortal(Portal portal)
        {
            portals.Add(portal);
            return true;
        }
        internal void DrawEntities(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, transformMatrix: transform);
            foreach (var item in entities)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.Draw(particles);
            player.Draw(spriteBatch);
            spriteBatch.End();
        }
        internal void DrawPortals(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: transform);
            foreach (var item in portals)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        internal void DrawPlatforms(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointWrap, transformMatrix: transform);
            foreach (var item in platforms)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        public bool AddPlatform(Platform entity)
        {
            platforms.Add(entity);
            return true;
        }
        internal void Update(GameTime gameTime)
        {
            foreach (var item in entities.ToList())
            {
                item.Update(gameTime);
                if (!item.Alive)
                {
                    entities.Remove(item);
                    continue;
                }
            }
            foreach (var item in platforms)
            {
                item.Update(gameTime);
            }
            foreach (var item in portals)
            {
                item.Update(gameTime);
            }
            foreach (var item in particles.Emitters.ToList())
            {
                //Console.WriteLine("asd");
                if (!item.Update((float)gameTime.ElapsedGameTime.TotalSeconds))
                {
                    particles.Emitters.Remove(item);
                }
            }
            player.Update(gameTime);
        }
        public List<Node> PathNodes = new();

        public Node? GetClosestNode(Vector2 pos, Node excluding = null)
        {
            if (PathNodes.Count == 0)
            {
                return null;
            }
            if (PathNodes.Count == 1)
            {
                return PathNodes[0];
            }
            if (excluding != null)
            {
                if (PathNodes.OrderBy(x => Vector2.DistanceSquared(pos, x.Position) / x.Weight).First() == excluding)
                {
                    return PathNodes.OrderBy(x => Vector2.DistanceSquared(pos, x.Position) / x.Weight).ToList()[1];
                }
            }
            return PathNodes.OrderBy(x => Vector2.DistanceSquared(pos, x.Position) / x.Weight).First();
        }

        internal bool HasPortalTo(Portal portal)
        {
            return portals.Any(x => x.TargetPortal == portal);
        }
        internal bool HasPortalTo(Room room)
        {
            return portals.Any(x => x.TargetPortal?.ContainingRoom == room);
        }

        internal Portal SetRandomPortalTo(Portal portal)
        {
            List<Portal> hasNoTarget = portals.Where(x => x.TargetPortal == null).ToList();
            if (hasNoTarget.Count == 0)
                throw new Exception("There are no empty portals left!");
            var randomPortal = hasNoTarget[RNG.Get(hasNoTarget.Count)];
            randomPortal.SetPortalTo(portal);
            return randomPortal;
        }
    }
}
