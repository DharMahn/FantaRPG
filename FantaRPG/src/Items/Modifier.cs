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
        public int ItemLevel { get; private set; }
        public static Modifier GenerateModifier(int ItemLevel)
        {
            Modifier modifier = new(ItemLevel);
            GenerateStats(ref modifier, ItemLevel);
            return modifier;
        }
        public override string ToString()
        {
            return $"ILvl: {ItemLevel}\n" + stats.ToString();
        }
        private static void GeneratePrimaryStats(ref Modifier modifier, int ilvl)
        {
            int num = Game1.Random.Next(0, Enum.GetValues(typeof(Stat)).Length - 1);
            modifier.stats.IncrementStat((Stat)num, ilvl);
        }
        private static void GenerateStats(ref Modifier modifier, int ilvl)
        {
            int times = Game1.Random.Next(1, 3);
            int pool = 100;
            int num;
            for (int i = 0; i < times - 1; i++)
            {
                int removeFromPool = Game1.Random.Next(10, pool - 10);
                num = Game1.Random.Next(0, Enum.GetValues(typeof(Stat)).Length - 1);
                modifier.stats.IncrementStat((Stat)num, ilvl * (pool / 100f) / times);
                pool -= removeFromPool;
            }
            num = Game1.Random.Next(0, Enum.GetValues(typeof(Stat)).Length - 1);
            modifier.stats.IncrementStat((Stat)num, ilvl * (pool / 100f) / times);
        }
        public Stats Stats { get { return stats; } }
        public Modifier()
        {
            stats = new Stats();
        }
        public Modifier(Stats stats)
        {
            this.stats = stats;
        }
        public Modifier(int ILvl) : this()
        {
            ItemLevel = ILvl;
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
