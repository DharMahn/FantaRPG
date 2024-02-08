using FantaRPG.src.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FantaRPG.src.Enemies
{
    internal class AIEnemy : Entity
    {
        private readonly float nodeCloseThreshold;
        private Node NextNode = null;
        private Node PrevNode = null;

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
