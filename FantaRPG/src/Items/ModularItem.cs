using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Items
{
    internal class ModularItem
    {
        List<Modifier> modifiers = new List<Modifier>();
        public void AddModifier(Modifier modifier)
        {
            modifiers.Add(modifier);
        }
        public Stats GetAllStats()
        {
            Stats returnStats = new Stats();
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
