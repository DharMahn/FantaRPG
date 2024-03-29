﻿using System;

namespace FantaRPG.src.Items
{
    internal class Modifier
    {
        public int Level { get; private set; }
        public static Modifier GenerateModifier(int ItemLevel)
        {
            Modifier modifier = new(ItemLevel);
            GenerateSecondaryStats(ref modifier, ItemLevel);
            return modifier;
        }
        public override string ToString()
        {
            return $"ILvl: {Level}" + Environment.NewLine + Stats.ToString();
        }
        private static void GeneratePrimaryStats(ref Modifier modifier, int ilvl)
        {
            int num = RNG.Get(0, Enum.GetValues(typeof(Stat)).Length - 1);
            modifier.Stats[(Stat)num] += ilvl / Stats.statValues[(Stat)num];
        }
        private static void GenerateSecondaryStats(ref Modifier modifier, int ilvl)
        {
            int times = RNG.Get(1, 5);
            float remainingIlvl = ilvl;

            for (int i = 0; i < times - 1; i++) // Loop until times - 1
            {
                float minSlice = 10f; // Minimum slice size
                float maxSlice = remainingIlvl - (minSlice * (times - i - 1)); // Maximum slice size
                float slice = ((float)RNG.GetDouble() * (maxSlice - minSlice)) + minSlice; // Get random value between min and max

                remainingIlvl -= slice;

                Stat selectedStat = (Stat)RNG.Get(0, Enum.GetValues(typeof(Stat)).Length - 1);
                float selectedStatMultiplier = Stats.statValues[selectedStat];
                float statContribution = slice / selectedStatMultiplier;

                modifier.AddToStats(selectedStat, statContribution);
            }

            // Use remaining ilvl for the last stat
            Stat lastStat = (Stat)RNG.Get(0, Enum.GetValues(typeof(Stat)).Length - 1);
            float lastStatMultiplier = Stats.statValues[lastStat];
            float lastStatContribution = remainingIlvl / lastStatMultiplier;

            modifier.AddToStats(lastStat, lastStatContribution);
        }
        public Stats Stats { get; }
        public Modifier()
        {
            Stats = new Stats();
        }
        public Modifier(Stats stats)
        {
            Stats = stats;
        }
        public Modifier(int ILvl) : this()
        {
            Level = ILvl;
        }

        public Modifier(Stat moveSpeed, int value) : this()
        {
            AddToStats(moveSpeed, value);
        }
        public void AddToStats(Stat stat, float value)
        {
            if (Stats[stat] == 0)
            {
                Stats[stat] = value;
                return;
            }
            Stats[stat] = value;
        }
    }
}
