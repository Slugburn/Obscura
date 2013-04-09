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

        public static IEnumerable<T> Less<T>(this IEnumerable<T> items, IEnumerable<T> toRemove)
        {
            var itemGroups = items.ToLookup(x => x);
            var removeGroups = toRemove.ToLookup(x => x);
            return itemGroups.Where(group => group.Count() > removeGroups[group.Key].Count())
                .SelectMany(group => group.Key.Repeat(group.Count() - removeGroups[group.Key].Count()));
        }

        public static IEnumerable<T> Less<T>(this IEnumerable<T> items, T toRemove)
        {
            var itemList = items.ToList();
            itemList.Remove(toRemove);
            return itemList;
        }
    }
}
