using FantaRPG.src.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Inventory
{
    internal class Inventory
    {
        List<Item> slots = new();
        public List<Item>? GetItem(Item item)
        {
            return slots.Where(x=>x.GetType() == item.GetType())?.ToList();
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
