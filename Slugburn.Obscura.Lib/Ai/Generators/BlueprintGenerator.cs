using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.Generators
{
    public class BlueprintGenerator
    {
        public IList<ShipPart> GetBestParts(ShipBlueprint blueprint, IList<ShipPart> partsPool)
        {
            var possibleParts = partsPool.Concat(new ShipPart[] {null}).ToArray();
            return Enumerable.Range(0, 1000)
                .Select(x => CreatePossiblePartList(blueprint, possibleParts))
                .Select(parts => new {Profile = ShipProfile.Create(blueprint, parts.ToArray()), Parts = parts})
                .Where(x => blueprint.IsProfileValid(x.Profile))
                .OrderByDescending(x => x.Profile.Rating)
                .First()
                .Parts
                .OrderBy(x=>x.Name)
                .ToList();
        }

        private static IList<ShipPart> CreatePossiblePartList(ShipBlueprint blueprint, IList<ShipPart> partsPool)
        {
            // Unique parts should not be replaced, so keep them constant
            // Also prioritize unique parts
            var staticParts =
                blueprint.Parts.Where(x => x.IsUnique)
                    .Concat(partsPool.Where(x => x!=null && x.IsUnique && blueprint.CanUsePartToUpgrade(x)))
                    .ToArray();

            var additional = CreateRandomPartList(blueprint.PartSpaces - staticParts.Length, partsPool);
            return staticParts.Concat(additional.Where(x => x != null)).ToArray();
        }

        private static IEnumerable<ShipPart> CreateRandomPartList(int partCount, IEnumerable<ShipPart> partsPool)
        {
            return Enumerable.Range(0, partCount).Select(x => partsPool.PickRandom()).ToList();
        }
    }
}
