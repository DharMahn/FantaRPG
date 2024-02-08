using System;

namespace FantaRPG.src
{
    internal static class RNG
    {
        private static readonly Random random = new();
        public static int Get(int fromInclusive, int toExclusive)
        {
            return random.Next(fromInclusive, toExclusive);
        }

        public static int Get(int toExclusive)
        {
            return random.Next(0, toExclusive);
        }

        public static double GetDouble()
        {
            return random.NextDouble();
        }

        public static double GetDouble(double start, double end)
        {
            return (random.NextDouble() * (end - start)) + start;
        }
    }
}
