using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    public enum Stat
    {
        MoveSpeed, JumpStrength, Armor, Damage, Health, Mana, Stamina
    }
    internal class Stats
    {
        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var item in stats)
            {
                sb.Append(item.Key);
                sb.Append(": ");
                sb.Append(item.Value);
                sb.AppendLine();
            }
            return sb.ToString();
        }
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
            return stats.TryGetValue(stat, out float value) ? value : 0;
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
