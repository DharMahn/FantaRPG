using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class Stats
    {
        private static HashSet<string> availableStats = new HashSet<string>()
        {
            "MoveSpeed",
        };
        private Dictionary<string, float> stats;
        public Stats()
        {
            stats = new Dictionary<string, float>();
        }
        public Stats(Dictionary<string, float> stats)
        {
            this.stats = stats;
        }
        public float GetStat(string name)
        {
            if (availableStats.Contains(name))
            {
                float value;
                return stats.TryGetValue(name, out value) ? value : default;
            }
            throw new Exception($"{name} does not exist as a stat");
        }
        public void AddStat(string name, float value)
        {
            if (availableStats.Contains(name))
            {
                stats[name] = value;
            }
        }
    }
}
