using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    public enum Stat
    {
        MoveSpeed, JumpStrength, Armor, Damage
    }
    internal class Stats
    {
        public Dictionary<Stat,float> GetAllStats()
        {
            return stats;
        }
        private Dictionary<Stat, float> stats;
        public Stats()
        {
            stats = new Dictionary<Stat, float>();
        }
        public Stats(Dictionary<Stat, float> stats)
        {
            this.stats = stats;
        }
        public float GetStat(Stat stat)
        {
            float value;
            return stats.TryGetValue(stat, out value) ? value : 0;
        }
        public void SetStat(Stat name, float value)
        {
            stats[name] = value;
        }

        internal void IncrementStat(Stat stat, float value)
        {
            if (stats.ContainsKey(stat))
            {
                stats[stat] += value;
                return;
            }
            stats[stat] = value;
        }
    }
}
