using System;
using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> input, Action<T> fn)
        {
            foreach (T item in input)
            {
                fn(item);
            }
        }

        public static IEnumerable<T2> CastAs<T1, T2>(this IEnumerable<T1> input, Func<T1, T2> fn)
        {
            var output = new List<T2>();
            input.ForEach(item => output.Add(fn(item)));
            return output;
        }
    }
}
