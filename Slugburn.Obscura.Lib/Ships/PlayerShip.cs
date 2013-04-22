using System.Linq;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ships
{
    public class PlayerShip : Ship, IBuildable
    {
        public PlayerShip(Faction faction, ShipBlueprint blueprint)
        {
            Owner = faction;
            Blueprint = blueprint;
        }

        public void Place(Sector sector)
        {
            sector.AddShip(this);
        }

        public bool IsPinned
        {
            get
            {
                var friendlyShipCount = Sector.Ships.Where(ship=>ship is PlayerShip).Cast<PlayerShip>().Count(ship => ship.Owner == Owner);
                var enemyShipCount = Sector.Ships.Count() - friendlyShipCount;
                return friendlyShipCount <= enemyShipCount;
            }
        }

        public ShipBlueprint Blueprint { get; private set; }

        public override ShipProfile Profile
        {
            get { return Blueprint.Profile; }
        }

        public override ShipType ShipType
        {
            get { return Blueprint.ShipType; }
        }

        protected override string Name
        {
            get { return Blueprint.Name; }
        }

    }
}