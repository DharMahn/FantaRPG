using Microsoft.Xna.Framework;
using System;
using System.Reflection;

namespace FantaRPG.src
{
    public static class Extensions
    {
        public static T GetFieldValue<T>(this object obj, string name)
        {
            // Set the flags so that private and public fields from instances will be found
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            FieldInfo field = obj.GetType().GetField(name, bindingFlags);
            return (T)field?.GetValue(obj);
        }
        public static Vector2 RotateVector(Vector2 vector, float degrees)
        {
            float radians = degrees * MathF.PI / 180;
            float sin = MathF.Sin(radians);
            float cos = MathF.Cos(radians);

            float tx = vector.X;
            float ty = vector.Y;

            return new Vector2((cos * tx) - (sin * ty), (sin * tx) + (cos * ty));
        }
    }
}
