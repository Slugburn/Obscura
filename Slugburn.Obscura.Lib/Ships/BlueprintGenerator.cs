using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;

namespace Slugburn.Obscura.Lib.Ships
{
    public class BlueprintGenerator
    {
        public IList<ShipPart> GetBestParts(ShipBlueprint blueprint, IList<ShipPart> partsPool)
        {
            return Enumerable.Range(0, 100).Select(x => CreateRandomPartList(blueprint.PartSpaces, partsPool))
                .Select(parts => new {Profile = ShipProfile.Create(blueprint, parts), Parts = parts})
                .Where(x => blueprint.IsProfileValid(x.Profile))
                .OrderByDescending(x => RateProfile(x.Profile))
                .First().Parts;
        }

        public IList<ShipPart> CreateRandomPartList(int partCount, IList<ShipPart> partsPool)
        {
            return Enumerable.Range(0, partCount).Select(x => partsPool.PickRandom()).ToList();
        }

        public double RateBlueprint(ShipBlueprint blueprint)
        {
            return RateProfile(blueprint.Profile); 
        }

        public double RateBlueprint(ShipBlueprint blueprint, IList<ShipPart> parts)
        {
            return RateProfile(ShipProfile.Create(blueprint, parts));
        }

        private double RateProfile(ShipProfile profile)
        {
            var damageRating = profile.Cannons.Sum() + (profile.Missiles.Sum()*0.75);
            var offenseMultiplier = (1 + profile.Accuracy + profile.Initiative*.25);
            var defenseMultipler = (1 + profile.Structure - profile.Deflection);
            return damageRating * offenseMultiplier * defenseMultipler + profile.Move;
        }
    }
}
