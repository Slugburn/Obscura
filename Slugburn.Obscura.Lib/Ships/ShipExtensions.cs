using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Obscura.Lib.Ships
{
    public static class ShipExtensions
    {
        public static decimal GetTotalRating(this IEnumerable<Ship> ships)
        {
            return ships.Sum(ship => ship.Rating);
        }

        public static string ListToString(this IEnumerable<ShipPart> shipParts)
        {
            return String.Join(", ", shipParts.GroupBy(p => p).Select(g => string.Format("{0}{1}", g.First().Name, g.Count() > 1 ? " x " + g.Count() : null)));
        }
    }
}
