using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Obscura.Lib.Extensions
{
    public static class RandomExtensions
    {
        private static readonly Random Random = new Random();

        public static List<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var list = source as List<T> ?? new List<T>(source);
            var shuffled = new List<T>();
            while (list.Count > 0)
            {
                var randomIndex = Random.Next(list.Count);
                shuffled.Add(list[randomIndex]);
                list.RemoveAt(randomIndex);
            }
            return shuffled;
        }

        public static List<T> Draw<T>(this IList<T> source, int count)
        {
            var drawn = source.Take(count).ToList();
            for (var i = 0; i < drawn.Count; i++)
                source.RemoveAt(0);
            return drawn;
        }

        public static T Draw<T>(this IList<T> source)
        {
            return Draw(source, 1).FirstOrDefault();
        }

        public static T PickRandom<T>(this IEnumerable<T> items)
        {
            var list = items as IList<T> ?? new List<T>(items);
            if (list.Count==0)
                throw new ArgumentException("Unable to pick random item from empty list");
            var randomIndex = Random.Next(list.Count);
            return list[randomIndex];
        }
    }
}
