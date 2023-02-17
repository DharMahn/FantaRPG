using FantaRPG.src.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Inventory
{
    internal class Slot
    {
        public Item Item { get; set; }
        public int Value { get; set; }
    }
}