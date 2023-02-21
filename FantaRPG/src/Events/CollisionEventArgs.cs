using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Events
{
    internal class CollisionEventArgs : EventArgs
    {
        public Action OnCollisionAction { get; protected set; }
        public CollisionEventArgs(Action action)
        {
            OnCollisionAction = action;
        }
    }
}
