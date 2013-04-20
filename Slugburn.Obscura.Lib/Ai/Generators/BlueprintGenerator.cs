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
            return Enumerable.Range(0, 1000)
                .Select(x => CreatePossiblePartList(blueprint, partsPool))
                .Select(parts => new {Profile = ShipProfile.Create(blueprint, parts.ToArray()), Parts = parts})
                .Where(x => blueprint.IsProfileValid(x.Profile))
                .OrderByDescending(x => x.Profile.Rating)
                .First()
                .Parts
                .OrderBy(x=>x.Name)
                .ToList();
        }

        private IList<ShipPart> CreatePossiblePartList(ShipBlueprint blueprint, IEnumerable<ShipPart> partsPool)
        {
            // Unique parts should not be replaced, so keep them constant
            var existingUniqueParts = blueprint.Parts.Where(x => x.IsUnique).ToArray();
            var additional = CreateRandomPartList(blueprint.PartSpaces - existingUniqueParts.Length, partsPool);
            return existingUniqueParts.Concat(additional).ToArray();
        }

        private static IEnumerable<ShipPart> CreateRandomPartList(int partCount, IEnumerable<ShipPart> partsPool)
        {
            return Enumerable.Range(0, partCount).Select(x => partsPool.PickRandom()).ToList();
        }
    }
}
