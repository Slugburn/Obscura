using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Actions;
using Slugburn.Obscura.Lib.Extensions;
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
            decimal value = sector.GetSectorRating();
            var distance = faction.Sectors.Min(x =>
                {
                    var shortest = faction.GetShortestPath(x, sector);
                    return shortest == null ? Int32.MinValue : shortest.Count;
                });
            if (distance > 3) return 0;
            var defenseRating = faction.GetEnemyRatingFor(sector);
            return value*100/defenseRating/(distance + 1);
        }

        public static int GetSectorRating(this Sector sector)
        {
            return sector.Vp + sector.Squares.Count + (sector.DiscoveryTile != null ? 2 : 0) +
                   sector.Ships.Sum(x => x is AncientShip ? 1 : x is GalacticCenterDefenseSystem ? 3 : 0);
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

        public static DecisionResult<IAction> ChooseBest(this IEnumerable<ActionRating> actionRatings, ILog log)
        {
            var possibleActions = actionRatings.Where(x => x.Action != null && x.Rating > 0).ToList();
            if (!possibleActions.Any())
                return new ActionDecisionResult(new ExploreDecision());

            possibleActions.Each(x => log.Log("\t\t{0} [{1:n2}]", x.Action, x.Rating));
            var bestRating = possibleActions.Max(x => x.Rating);
            var action = possibleActions.Where(x=>x.Rating==bestRating).PickRandom().Action;
            return new ActionDecisionResult(action);
        }

        public static bool SpendingInfluenceWillBankrupt(this IAiPlayer player)
        {
            var faction = player.Faction;
            if (faction.Influence == 0)
                return true;
            var income = faction.GetIncomeForInfluence(faction.Influence - 1) + GetEmergencyIncome(player);

            return income < 0;
        }

        private static int GetEmergencyIncome(IAiPlayer player)
        {
            var faction = player.Faction;
            if (faction.Game.Round == 10 || faction.Sectors.Any(x=>x.GetEnemyShips(faction).Any()))
            {
                return faction.Material/faction.TradeRatio + faction.Science/faction.TradeRatio;
            }
            return 0;
        }

        public static int GetActionsBeforeBankruptcy(this IAiPlayer player)
        {
            var faction = player.Faction;
            var actions = 0;
            while (faction.GetIncomeForInfluence(faction.Influence - actions) + GetEmergencyIncome(player) >= 0)
                actions++;
            return actions;
        }
    }
}
