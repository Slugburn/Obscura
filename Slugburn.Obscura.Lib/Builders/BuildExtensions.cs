using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Builders
{
    public static class BuildExtensions
    {
        public static decimal GetTotalCombatRatingFor(this IEnumerable<IBuilder> builders, PlayerFaction faction)
        {
            return builders.Sum(x=>x.CombatRatingFor(faction));
        }
    }
}
