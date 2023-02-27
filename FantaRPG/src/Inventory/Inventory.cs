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
        List<Item> slots = new List<Item>();
        public List<Item>? GetItem(Item item)
        {
            return slots.Where(x=>x.GetType() == item.GetType())?.ToList();
        }
    }
}
