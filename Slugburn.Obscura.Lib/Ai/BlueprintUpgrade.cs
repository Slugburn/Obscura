using System.Collections.Generic;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai
{
    public class BlueprintUpgrade
    {
        public ShipBlueprint Blueprint { get; set; }

        public ShipPart Upgrade { get; set; }

        public ShipPart Replace { get; set; }

        public decimal RatingImprovement { get; set; }

        public List<ShipPart> Before { get; set; }

        public List<ShipPart> After { get; set; }

        public int Order { get; set; }
    }
}