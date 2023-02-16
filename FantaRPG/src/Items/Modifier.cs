using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src.Items
{
    internal class Modifier
    {
        private Stats stats;

        public Stats Stats { get { return stats; } }
        public Modifier()
        {
            stats = new Stats();
        }
        public Modifier(Stats stats) : this()
        {
            this.stats = stats;
        }

        public Modifier(Stat moveSpeed, int value) : this()
        {
            AddToStats(moveSpeed, value);
        }

        public void AddToStats(Stat stat, float value)
        {
            if (stats.GetStat(stat) == 0)
            {
                stats.SetStat(stat, value);
                return;
            }
            stats.IncrementStat(stat, value);
        }
        public void SetStat(Stat stat, float value)
        {
            stats.SetStat(stat, value);
        }
    }
}
