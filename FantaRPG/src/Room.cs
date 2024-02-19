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
        public List<BackgroundLayer> Backgrounds { get; }
        public List<Platform> Platforms { get; }
        public List<Entity> Entities { get; }
        private readonly ParticleEffect particles;
        public List<Portal> Portals { get; }
        public float Gravity { get; private set; }
        public Point Bounds { get; protected set; }
        public void AddEmitter(ParticleEmitter effect)
        {
            particles.Emitters.Add(effect);
        }
        public bool AddEntity(Entity entity)
        {
            Entities.Add(entity);
            return true;
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
                    Entities.Remove(item);
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
                    particles.Emitters.Remove(item);
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
        internal Portal GetRandomEmptyPortal()
        {
            // Filter for portals that do not have a target
            List<Portal> availablePortals = Portals.Where(portal => portal.TargetPortal == null).ToList();

            if (availablePortals.Count == 0)
            {
                throw new Exception("There are no empty portals available.");
            }

            // Select and return a random empty portal
            return availablePortals[RNG.Get(availablePortals.Count)];
        }

        internal Portal SetRandomPortalTo(Room targetRoom)
        {
            // Find all empty portals in the current room
            List<Portal> availablePortals = Portals.Where(x => x.TargetPortal == null).ToList();
            if (availablePortals.Count == 0)
            {
                throw new Exception("There are no empty portals left in the current room!");
            }

            // Select a random empty portal from the current room
            Portal randomPortalInCurrentRoom = availablePortals[RNG.Get(availablePortals.Count)];

            // Attempt to set this portal to connect to a portal in the target room.
            // This action will attempt to find an empty portal in the target room to link back to this portal,
            // thereby potentially creating a two-way connection.
            randomPortalInCurrentRoom.SetPortalTo(targetRoom);

            return randomPortalInCurrentRoom;
        }
    }
}
