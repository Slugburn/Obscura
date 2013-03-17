using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Obscura.Lib.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> items, Action<T> action )
        {
            foreach (var item in items)
                action(item);
            return items;
        }

        public static IEnumerable<T> Repeat<T>(this T item, int count)
        {
            return Enumerable.Repeat(item, count);
        }
    }
}
