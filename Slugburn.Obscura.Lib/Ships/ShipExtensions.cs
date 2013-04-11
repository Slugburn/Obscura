using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Obscura.Lib.Ships
{
    public static class ShipExtensions
    {
        public static double GetTotalRating(this IEnumerable<Ship> ships)
        {
            return ships.Sum(ship => ship.Profile.Rating);
        }
    }
}
