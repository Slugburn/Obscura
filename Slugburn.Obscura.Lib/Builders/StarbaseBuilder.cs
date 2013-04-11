using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public class StarbaseBuilder : ShipBuilder
    {
        public StarbaseBuilder() : base(faction=>faction.Starbase, 4)
        {
        }

        public override bool IsBuildAvailable(Faction faction)
        {
            var baseResult = base.IsBuildAvailable(faction);
            return baseResult 
                && faction.HasTechnology(Tech.Starbase)
                && faction.Sectors.Any(IsValidPlacementLocation);
        }

        public override bool IsValidPlacementLocation(Sector sector)
        {
            return sector.Ships
                         .Cast<PlayerShip>().All(ship => ship.Blueprint != sector.Owner.Starbase);
        }

        public override bool OnePerSector
        {
            get { return true; }
        }
    }
}
