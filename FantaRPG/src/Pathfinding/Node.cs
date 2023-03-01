using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Pathfinding
{
    internal class Node
    {
        public Vector2 Position;
        public float Weight = 1;
        public List<Node> Neighbours = new List<Node>();
        public Node(Vector2 pos)
        {
            Position = pos;
        }
        public Node(Vector2 pos, float weight)
        {
            Position = pos;
            Weight = weight;
        }
        public Node? GetClosestNode()
        {
            if (Neighbours.Count == 0)
            {
                return null;
            }
            if (Neighbours.Count == 1)
            {
                return Neighbours[0];
            }
            return Neighbours.OrderBy(x => Vector2.DistanceSquared(Position, x.Position) / Weight).FirstOrDefault();
        }
    }
}
