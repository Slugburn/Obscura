using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai
{
    public class ShipMove
    {
        public ShipMove(PlayerShip ship, Sector destination)
        {
            Ship = ship;
            Destination = destination;
        }

        public PlayerShip Ship { get; set; }
        public Sector Destination { get; set; }
    }
}