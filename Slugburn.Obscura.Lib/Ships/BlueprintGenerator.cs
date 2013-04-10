﻿using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;

namespace Slugburn.Obscura.Lib.Ships
{
    public class BlueprintGenerator
    {
        public IList<ShipPart> GetBestParts(ShipBlueprint blueprint, IList<ShipPart> partsPool)
        {
            return Enumerable.Range(0, 1000).Select(x => CreateRandomPartList(blueprint.PartSpaces, partsPool))
                .Select(parts => new {Profile = ShipProfile.Create(blueprint, parts), Parts = parts})
                .Where(x => blueprint.IsProfileValid(x.Profile))
                .OrderByDescending(x => x.Profile.Rating)
                .First().Parts;
        }

        public IList<ShipPart> CreateRandomPartList(int partCount, IList<ShipPart> partsPool)
        {
            return Enumerable.Range(0, partCount).Select(x => partsPool.PickRandom()).ToList();
        }

        public double RateBlueprint(ShipBlueprint blueprint)
        {
            return blueprint.Profile.Rating; 
        }

        public double RateBlueprint(ShipBlueprint blueprint, IList<ShipPart> parts)
        {
            return ShipProfile.Create(blueprint, parts).Rating;
        }
    }
}
