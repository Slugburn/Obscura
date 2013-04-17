using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai
{
    public class MoveListGenerator
    {
        public IList<ShipMove> Generate(IAiPlayer player)
        {
            return player.Faction.Ships
                .Where(ship => ship.Move > 0 && ship.Sector != player.RallyPoint)
                .Select(ship => new {ship, path = player.Faction.GetShortestPath(ship.Sector, player.RallyPoint)})
                .Where(x=>x.path != null)
                .SelectMany(x => GenerateShipMoves(x))
                .ToList();
        }

        private static IEnumerable<ShipMove> GenerateShipMoves(dynamic dyn)
        {
            var segments = new List<List<Sector>>();
            var segment = new List<Sector>();
            PlayerShip ship = dyn.ship;
            foreach (Sector sector in dyn.path)
            {
                segment.Add(sector);
                if (segment.Count == ship.Move)
                {
                    segments.Add(segment);
                    segment = new List<Sector>();
                }
            }
            if (segment.Any())
                segments.Add(segment);

            return segments.Select(x=>new ShipMove(ship, x));
        }
    }
}