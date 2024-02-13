using System;
using System.Collections.Generic;
using System.Text;

namespace FantaRPG.src
{
    public enum Stat
    {
        MoveSpeed, JumpStrength, Armor, Damage, Health, Mana, Stamina
    }
    internal class Stats
    {
        public static readonly Dictionary<Stat, float> statValues = new()
        {
            {Stat.MoveSpeed,    2},
            {Stat.JumpStrength, 4},
            {Stat.Mana,         1},
            {Stat.Damage,       1},
            {Stat.Armor,        1},
            {Stat.Health,       1},
            {Stat.Stamina,      1},
        };
        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (KeyValuePair<Stat, float> item in stats)
            {
                sb.Append(item.Key);
                sb.Append(": ");
                sb.Append(item.Value);
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
        public Dictionary<Stat, float> GetAllStats()
        {
            return stats;
        }
        private readonly Dictionary<Stat, float> stats;
        public Stats()
        {
            stats = [];
        }
        public Stats(Dictionary<Stat, float> stats)
        {
            this.stats = stats;
        }
        public float this[Stat stat]
        {
            get
            {
                return stats.TryGetValue(stat, out float value) ? value : 0;
            }
            set
            {
                stats[stat] = value;
            }
        }
    }
}
