using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Actions
{
    public class MoveAction : IAction
    {
        private readonly ILog _log;

        public MoveAction(ILog log)
        {
            _log = log;
        }

        public override string ToString()
        {
            return "Move";
        }

        public void Do(PlayerFaction faction)
        {
            var movesCompleted = 0;
            while (movesCompleted < faction.MoveCount)
            {
                var moveable = GetMoveableShips(faction).ToArray();
                if (!moveable.Any())
                    break;
                var ship = faction.Player.ChooseShipToMove(moveable);
                if (ship == null)
                    break;
                if (moveable.All(x => x != ship))
                    throw new InvalidOperationException(string.Format("{0} in {1} is not moveable", ship, ship.Sector));
                var validDestinations = GetValidDestinations(ship).ToList();
                if (!validDestinations.Any())
                    break;
                var path = faction.Player.ChooseShipPath(ship, validDestinations);
                if (path.Count > ship.Move)
                    throw new InvalidOperationException(string.Format("{0} is not capable of moving {1}", ship, path.Count));
                foreach (var sector in path)
                {
                    if (validDestinations.All(x => x != sector))
                        throw new InvalidOperationException(string.Format("Moving {0} in {1} to {2} is not valid", ship, ship.Sector, sector));
                    _log.Log("\t{0}: {1} => {2}", ship, ship.Sector, sector);
                    sector.AddShip(ship);
                }
                movesCompleted++;
            }
        }

        private static IEnumerable<PlayerShip> GetMoveableShips(PlayerFaction faction)
        {
            return faction.Ships.Where(ship => ship.Move > 0 && !ship.IsPinned);
        }

        private IEnumerable<Sector> GetValidDestinations(PlayerShip ship)
        {
            return GetSectorsInRadius(ship.Sector, ship.Move);
        }

        private static IEnumerable<Sector> GetSectorsInRadius(Sector sector, int radius)
        {
            var adjacentSectors = sector.AdjacentSectors().ToArray();
            if (radius<=1)
                return adjacentSectors;
            return adjacentSectors.Concat(adjacentSectors.SelectMany(s => GetSectorsInRadius(s, radius - 1))).Distinct().Except(new[] {sector});
        }

        public bool IsValid(PlayerFaction faction)
        {
            var moveable = GetMoveableShips(faction);
            return moveable.Any(ship=>GetValidDestinations(ship).Any());
        }
    }
}