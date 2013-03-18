using System.Linq;

namespace Slugburn.Obscura.Lib.Ships
{
    public class Ship
    {
        public Sector Sector { get; set; }

        public void SetSector(Sector sector)
        {
            sector.AddShip(this);
            Sector = sector;
        }
    }

    public class PlayerShip : Ship
    {
        private readonly ShipBlueprint _blueprint;

        public PlayerShip(Player player, ShipBlueprint blueprint)
        {
            Player = player;
            _blueprint = blueprint;
        }

        public bool IsPinned
        {
            get
            {
                var friendlyShipCount = Sector.Ships.Cast<PlayerShip>().Count(ship => ship != null && ship.Player == Player);
                var enemyShipCount = Sector.Ships.Count() - friendlyShipCount;
                return friendlyShipCount > enemyShipCount;
            }
        }

        public Player Player { get; private set; }
    }
}