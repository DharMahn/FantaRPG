using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal static class RNG
    {
        private static Random random = new Random();
        public static int Get(int lower, int higher) => random.Next(lower, higher);
        public static double GetDouble() => random.NextDouble();
        public static double GetDouble(double start, double end) => random.NextDouble() * (end - start) + start;
    }
}
