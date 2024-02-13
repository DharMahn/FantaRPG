using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FantaRPG.src.Items
{
    internal class ModularItem(string name, Texture2D texture = null) : Item(name, texture)
    {
        private readonly List<Modifier> modifiers = [];

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
                foreach (KeyValuePair<Stat, float> stat in item.Stats.GetAllStats())
                {
                    returnStats[stat.Key] += stat.Value;
                }
            }
            return returnStats;
        }
    }
}
