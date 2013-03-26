using System.Linq;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Ships
{
    public class PlayerShip : Ship
    {
        private readonly ShipBlueprint _blueprint;

        public PlayerShip(Faction faction, ShipBlueprint blueprint)
        {
            Faction = faction;
            _blueprint = blueprint;
        }

        public bool IsPinned
        {
            get
            {
                var friendlyShipCount = Sector.Ships.Cast<PlayerShip>().Count(ship => ship != null && ship.Faction == Faction);
                var enemyShipCount = Sector.Ships.Count() - friendlyShipCount;
                return friendlyShipCount > enemyShipCount;
            }
        }

        public Faction Faction { get; private set; }
    }
}