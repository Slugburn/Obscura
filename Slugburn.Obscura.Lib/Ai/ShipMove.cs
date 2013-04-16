using System.Collections.Generic;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai
{
    public class ShipMove
    {
        public ShipMove(PlayerShip ship, IList<Sector> destination)
        {
            Ship = ship;
            Moves = destination;
        }

        public PlayerShip Ship { get; set; }
        public IList<Sector> Moves { get; set; }
    }
}