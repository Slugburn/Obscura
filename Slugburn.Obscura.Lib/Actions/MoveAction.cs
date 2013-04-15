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
                var validDestinations = GetValidDestinations(ship).ToList();
                if (!validDestinations.Any())
                    break;
                var destination = faction.Player.ChooseShipDestination(ship, validDestinations);

                _log.Log("{0} moves {1} from {2} to {3}",
                    faction, ship, ship.Sector, destination);

                destination.AddShip(ship);
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