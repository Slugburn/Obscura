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
            Faction = faction;
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
                var friendlyShipCount = Sector.Ships.Where(ship=>ship is PlayerShip).Cast<PlayerShip>().Count(ship => ship.Faction == Faction);
                var enemyShipCount = Sector.Ships.Count() - friendlyShipCount;
                return friendlyShipCount <= enemyShipCount;
            }
        }

        public ShipBlueprint Blueprint { get; private set; }

        public int Move
        {
            get { return Blueprint.Profile.Move; }
        }

        public override ShipProfile Profile
        {
            get { return Blueprint.Profile; }
        }

        public override ShipType ShipType
        {
            get { return Blueprint.ShipType; }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Blueprint, Profile);
        }

    }
}