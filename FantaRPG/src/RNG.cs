using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal static class RNG
    {
        private static readonly Random random = new();
        public static int Get(int fromInclusive, int toExclusive) => random.Next(fromInclusive, toExclusive);
        public static int Get(int toExclusive) => random.Next(0, toExclusive);
        public static double GetDouble() => random.NextDouble();
        public static double GetDouble(double start, double end) => random.NextDouble() * (end - start) + start;
    }
}
