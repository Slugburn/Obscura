using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.Generators
{
    public class MoveListGenerator
    {
        public IList<ShipMove> Generate(IAiPlayer player)
        {
            var faction = player.Faction;
            return faction.Ships
                .Where(ship => ShipShouldMove(player, ship))
                .Select(ship => new {ship, path = faction.GetShortestPath(ship.Sector, player.RallyPoint)})
                .Where(x=>x.path != null)
                .SelectMany(x => GenerateShipMoves(x))
                .GroupBy(x=>x.Ship)
                // move ships that can get there in the least number of moves first
                .OrderBy(g=>g.Count())
                // then consider the ship rating
                .ThenByDescending(g=>g.Key.Rating)
                .SelectMany(g=>g)
                .ToList();
        }

        private static bool ShipShouldMove(IAiPlayer player, PlayerShip ship)
        {
            return ship.Move > 0 // ship can move
                && ship.Sector != player.RallyPoint // ship is not already at rally point
                && !ship.Sector.GetEnemyShips(player.Faction).Any(); // ship is not already engaged with enemy
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

        public decimal Rate(IAiPlayer player)
        {
            return Rate(player, player.Faction.MoveCount);
        }

        public decimal Rate(IAiPlayer player, int moveCount)
        {
            if (player.GetAction<MoveAction>() == null)
                return 0;
            player.MoveList = Generate(player);
            return player.MoveList.Take(moveCount).Where(x => x.Moves.Last() == player.RallyPoint).Sum(x => x.Ship.Rating);
        }
    }
}