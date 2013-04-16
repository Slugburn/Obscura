using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;
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
                .Select(ship => new {ship, path = GetPath(player.Faction, ship.Sector, player.RallyPoint)})
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

        public static IList<Sector> GetPath(PlayerFaction faction, Sector start, Sector destination)
        {
            var shortest = int.MaxValue;
            var shortestPath = GetShortestPath(faction, start, destination, new Sector[0], ref shortest).Skip(1).ToList();
            return shortestPath;
        }

        private static IList<Sector> GetShortestPath(PlayerFaction faction, Sector start, Sector destination, IEnumerable<Sector> path, ref int shortest)
        {
            path = path.Concat(new[]{start}).ToArray();
            if (path.Count() > 10)
                return null;
            if (path.Count() >= shortest)
                return null;
            if (start == destination)
            {
                shortest = path.Count();
                return path.ToList();
            }
            IEnumerable<Sector> thePath = null;
            foreach (var adjacent in start.AdjacentSectors().Where(s=>s==destination || !s.GetEnemyShips(faction).Any()).Except(path))
            {
                var adjPath = GetShortestPath(faction, adjacent, destination, path, ref shortest);
                if (adjPath != null && adjPath.Count <= shortest)
                    thePath = adjPath;
            }
            return thePath != null ? thePath.ToList()  : new List<Sector>();
        }
    }
}