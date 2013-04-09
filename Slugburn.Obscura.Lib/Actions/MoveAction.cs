using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Actions
{
    public class MoveAction : IAction
    {
        public void Do(Faction faction)
        {
            var movesCompleted = 0;
            while (movesCompleted < faction.MoveCount)
            {
                var ship = faction.Player.ChooseShipToMove(GetMoveableShips(faction));
                if (ship == null)
                    break;
                var validDestinations = GetValidDestinations(ship);
                var destination = faction.Player.ChooseShipDestination(ship, validDestinations);
                destination.AddShip(ship);
                movesCompleted++;
            }
        }

        private static IEnumerable<PlayerShip> GetMoveableShips(Faction faction)
        {
            return faction.Ships.Where(x=>!x.IsPinned);
        }

        private IList<Sector> GetValidDestinations(PlayerShip ship)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValid(Faction faction)
        {
            return GetMoveableShips(faction).Any();
        }
    }
}