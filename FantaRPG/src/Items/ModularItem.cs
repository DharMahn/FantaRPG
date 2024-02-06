using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Items
{
    internal class ModularItem : Item
    {
        readonly List<Modifier> modifiers = new();

        public ModularItem(string name, Texture2D texture = null) : base(name, texture)
        {
        }
        public void AddModifier(Modifier modifier)
        {
            modifiers.Add(modifier);
        }
        public void RemoveModifier(Modifier modifier)
        {
            modifiers.Remove(modifier);
        }
        public Stats GetAllStats()
        {
            Stats returnStats = new();
            foreach (Modifier item in modifiers)
            {
                foreach (KeyValuePair<Stat,float> stat in item.Stats.GetAllStats())
                {
                    returnStats.IncrementStat(stat.Key, stat.Value);
                }
            }
            return returnStats;
        }
    }
}
