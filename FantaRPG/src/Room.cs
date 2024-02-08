using FantaRPG.src.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FantaRPG.src
{
    internal class Room
    {
        public static List<(Room Room, bool IsValid)> Rooms = [];

        public List<BackgroundLayer> Backgrounds { get; }
        public List<Platform> Platforms { get; }

        public List<Entity> Entities { get; }

        private readonly ParticleEffect particles;

        public List<Portal> Portals { get; }
        public float Gravity { get; private set; }
        public void AddEmitter(ParticleEmitter effect)
        {
            particles.Emitters.Add(effect);
        }
        public Point Bounds { get; protected set; }
        public bool AddEntity(Entity entity)
        {
            Entities.Add(entity);
            return true;
        }
        public static Room GetRandomValidRoom()
        {
            List<(Room Room, bool IsValid)> validRooms = Rooms.Where(x => x.IsValid).ToList();
            (Room Room, bool IsValid) selectedRoom = validRooms[RNG.Get(validRooms.Count())];
            selectedRoom.IsValid = false;
            return selectedRoom.Room;
        }
        public static void ResetRoomValidities()
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                Room room = Rooms[i].Room;
                Rooms[i] = (room, false);
            }
        }
        public Room(List<BackgroundLayer> bgs, List<Platform> platforms, List<Entity> entities, List<Portal> portals, Player player, Point bounds = new Point(), float gravity = 1f)
        {
            Backgrounds = bgs;
            Platforms = platforms;
            Entities = entities;
            Portals = portals;
            Gravity = gravity;
            particles = new ParticleEffect(autoTrigger: false)
            {
                Position = Vector2.Zero
            };
            Player = player;
            Bounds = bounds;
        }

        public Player Player { get; private set; }

        internal void DrawBackground(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);
            foreach (BackgroundLayer item in Backgrounds)
            {
                item.Draw(spriteBatch, transform);
            }
            spriteBatch.End();
        }
        public bool AddPortal(Portal portal)
        {
            Portals.Add(portal);
            return true;
        }
        internal void DrawEntities(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, transformMatrix: transform);
            foreach (Entity item in Entities)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.Draw(particles);
            Player.Draw(spriteBatch);
            spriteBatch.End();
        }
        internal void DrawPortals(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: transform);
            foreach (Portal item in Portals)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        internal void DrawPlatforms(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointWrap, transformMatrix: transform);
            foreach (Platform item in Platforms)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        public bool AddPlatform(Platform entity)
        {
            Platforms.Add(entity);
            return true;
        }
        internal void Update(GameTime gameTime)
        {
            foreach (Entity item in Entities.ToList())
            {
                item.Update(gameTime);
                if (!item.Alive)
                {
                    _ = Entities.Remove(item);
                    continue;
                }
            }
            foreach (Platform item in Platforms)
            {
                item.Update(gameTime);
            }
            foreach (Portal item in Portals)
            {
                item.Update(gameTime);
            }
            foreach (ParticleEmitter item in particles.Emitters.ToList())
            {
                //Console.WriteLine("asd");
                if (!item.Update((float)gameTime.ElapsedGameTime.TotalSeconds))
                {
                    _ = particles.Emitters.Remove(item);
                }
            }
            Player.Update(gameTime);
        }
        public List<Node> PathNodes = [];

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
            return Portals.Any(x => x.TargetPortal == portal);
        }
        internal bool HasPortalTo(Room room)
        {
            return Portals.Any(x => x.TargetPortal?.ContainingRoom == room);
        }

        internal Portal SetRandomPortalTo(Portal portal)
        {
            List<Portal> hasNoTarget = Portals.Where(x => x.TargetPortal == null).ToList();
            if (hasNoTarget.Count == 0)
            {
                throw new Exception("There are no empty portals left!");
            }

            Portal randomPortal = hasNoTarget[RNG.Get(hasNoTarget.Count)];
            randomPortal.SetPortalTo(portal);
            return randomPortal;
        }
    }
}
