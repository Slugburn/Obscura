using System.Linq;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public class StarbaseBuilder : ShipBuilder, IOnePerSectorBuilder
    {
        public StarbaseBuilder() : base("Starbase", faction=>faction.Starbase, 4)
        {
        }

        public override bool IsValidPlacementLocation(Sector sector)
        {
            return sector.Ships
                         .Cast<PlayerShip>().All(ship => ship.Blueprint != sector.Owner.Starbase);
        }

        public override Tech RequiredTech
        {
            get { return Tech.Starbase; }
        }

        public override bool CanMove
        {
            get { return false; }
        }

        public bool HasBeenBuilt(Sector sector)
        {
            return sector.Ships
                         .Cast<PlayerShip>().Any(ship => ship.Blueprint == sector.Owner.Starbase);
        }
    }
}
