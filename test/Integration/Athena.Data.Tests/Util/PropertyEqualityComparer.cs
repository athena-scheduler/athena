using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Athena.Data.Tests.Util
{
    public class PropertyEqualityComparer<T> : IEqualityComparer<T>
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;
        
        public bool Equals(T x, T y)
        {
            if (x == null || y == null) return false;

            return typeof(T).GetProperties(Flags).All(p => p.GetValue(x).Equals(p.GetValue(y))) &&
                   typeof(T).GetFields(Flags).All(f => f.GetValue(x).Equals(f.GetValue(y)));
        }

        public int GetHashCode(T obj) =>
            typeof(T).GetFields(Flags).Select(f => f.GetValue(obj).GetHashCode())
                .Concat(typeof(T).GetProperties(Flags).Select(f => f.GetValue(obj).GetHashCode()))
                .Aggregate(0, (result, hash) => result ^ hash);
    }
}