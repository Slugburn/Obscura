using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai
{
    public static class AiExtensions
    {
        public static IAction GetAction<T>(this IAiPlayer player) where T:IAction
        {
            return player.ValidActions.SingleOrDefault(a => a is T);
        }

        public static bool FleetCanConquer(this PlayerFaction faction, Sector targetSector)
        {
            if (faction.GetShortestPath(faction.HomeSector, targetSector) == null)
                return false;
            var enemyRating = faction.GetEnemyRatingFor(targetSector);
            var fleetRating = faction.Ships.Where(ship => IsShipAvailableToFightAt(ship, faction, targetSector)).GetTotalRating();
            var combatRatio = fleetRating/enemyRating;
            var fleetCombatRatio = combatRatio >= 2.0m;
            return fleetCombatRatio;
        }

        private static bool IsShipAvailableToFightAt(PlayerShip ship, PlayerFaction faction, Sector targetSector)
        {
            return ship.Sector == targetSector || !ship.Sector.GetEnemyShips(faction).Any();
        }

        public static decimal GetEnemySectorRating(this PlayerFaction faction, Sector sector)
        {
            decimal value = sector.Vp + sector.Squares.Count + (sector.DiscoveryTile != null ? 2 : 0);
            var distance = faction.Sectors.Min(x =>
                {
                    var shortest = faction.GetShortestPath(x, sector);
                    return shortest == null ? Int32.MinValue : shortest.Count;
                });
            if (distance > 3) return 0;
            var defenseRating = faction.GetEnemyRatingFor(sector);
            return value*100/defenseRating/(distance + 1);
        }

        public static decimal CombatSuccessRatio(this PlayerFaction playerFaction, Sector mySector, IEnumerable<Sector> enemySectors)
        {
            var friendlyRating = mySector.GetFriendlyShips(playerFaction).GetTotalRating();
            var enemyRating = enemySectors.Sum(sector => playerFaction.GetEnemyRatingFor(sector));
            return friendlyRating/enemyRating;
        }

        public static decimal CombatSuccessRatio(this PlayerFaction playerFaction, Sector sector)
        {
            return CombatSuccessRatio(playerFaction, sector, new[] {sector});
        }

        public static decimal GetEnemyRatingFor(this PlayerFaction faction, Sector sector)
        {
            var rating = sector.GetEnemyShips(faction).GetTotalRating();
            return rating > 1 ? rating : 1;
        }
    }
}
