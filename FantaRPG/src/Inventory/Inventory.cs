using FantaRPG.src.Items;
using System.Collections.Generic;
using System.Linq;

namespace FantaRPG.src.Inventory
{
    internal class Inventory
    {
        private readonly List<Item> slots = [];
        public List<Item>? GetItem(Item item)
        {
            return slots.Where(x => x.GetType() == item.GetType())?.ToList();
        }
        public List<Item> GetItems()
        {
            return slots;
        }

        internal void AddItem(Item item)
        {
            slots.Add(item);
        }
    }
}
