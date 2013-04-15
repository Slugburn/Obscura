using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ai
{
    public class MoveListGenerator
    {
        public IList<ShipMove> Generate(IAiPlayer player)
        {
            return player.Faction.Ships
                .Where(ship => ship.Move > 0 && ship.Sector != player.RallyPoint)
                .Select(ship => new {ship, path = GetPath(player.Faction, ship.Sector, player.RallyPoint)})
                .SelectMany(x => x.path.Select(s => new ShipMove(x.ship, s)))
                .ToList();
        }

        private IList<Sector> GetPath(PlayerFaction faction, Sector start, Sector destination)
        {
            var shortest = int.MaxValue;
            var shortestPath = GetShortestPath(faction, start, destination, new Sector[0], ref shortest).Skip(1).ToList();
            return shortestPath;
        }

        private IList<Sector> GetShortestPath(PlayerFaction faction, Sector start, Sector destination, IEnumerable<Sector> path, ref int shortest)
        {
            path = path.Concat(new[]{start}).ToArray();
            if (path.Count() >= shortest)
                return null;
            if (start == destination)
            {
                shortest = path.Count();
                return path.ToList();
            }
            IEnumerable<Sector> thePath = null;
            foreach (var adjacent in start.AdjacentSectors().Where(s=>!s.GetEnemyShips(faction).Any()).Except(path))
            {
                var adjPath = GetShortestPath(faction, adjacent, destination, path, ref shortest);
                if (adjPath != null && adjPath.Count <= shortest)
                    thePath = adjPath;
            }
            return thePath != null ? thePath.ToList()  : new List<Sector>();
        }
    }
}