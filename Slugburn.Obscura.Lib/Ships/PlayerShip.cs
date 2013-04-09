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

        public string Name { get { return Blueprint.Name; } }

        public void Place(Sector sector)
        {
            sector.AddShip(this);
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

        public ShipBlueprint Blueprint { get; private set; }
        
    }
}