using FantaRPG.src.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Enemies
{
    internal class AIEnemy : Entity
    {
        float nodeCloseThreshold;
        Node NextNode = null;
        Node PrevNode = null;

        public AIEnemy(float x, float y, Vector2 size, Texture2D texture = null) : base(x, y, size, texture)
        {
            nodeCloseThreshold = hitboxSize.X * 2;
            NextNode = Game1.Instance.CurrentRoom.GetClosestNode(position);
        }
        public override void Update(GameTime gameTime)
        {
            if (Vector2.DistanceSquared(NextNode.Position, Position) < nodeCloseThreshold * nodeCloseThreshold)
            {
                PrevNode = NextNode;
                NextNode = Game1.Instance.CurrentRoom.GetClosestNode(position, PrevNode);
            }
            else
            {

            }
        }
    }
}
