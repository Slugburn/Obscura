using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.StateMachine;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai
{
    public static class AiExtensions
    {
        public static IAction GetAction<T>(this IAiPlayer player) where T:IAction
        {
            return player.ValidActions.SingleOrDefault(a => a is T);
        }

        public static bool FleetCanConquer(this Faction faction, Sector targetSector)
        {
            if (faction.GetShortestPath(faction.HomeSector, targetSector) == null)
                return false;
            var enemyRating = faction.GetEnemyRatingFor(targetSector);
            var fleetRating = faction.Ships.Where(ship => IsShipAvailableToFightAt(ship, faction, targetSector)).GetTotalRating();
            var combatRatio = fleetRating/enemyRating;
            var fleetCombatRatio = combatRatio >= 2.0m;
            return fleetCombatRatio;
        }

        private static bool IsShipAvailableToFightAt(PlayerShip ship, Faction faction, Sector targetSector)
        {
            return ship.Sector == targetSector || !ship.Sector.GetEnemyShips(faction).Any();
        }

        public static decimal GetEnemySectorRating(this Faction faction, Sector sector)
        {
            decimal value = sector.GetSectorRating() ^ 2;
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
            var rating = sector.Vp 
                + sector.Squares.Count 
                + (sector.DiscoveryTile != null ? 2 : 0) 
                + sector.Ships.Sum(x => x is AncientShip ? 1 : x is GalacticCenterDefenseSystem ? 3 : 0);
            if (rating < 3)
                return 1;
            return rating;
        }

        public static decimal CombatSuccessRatio(this Faction faction, Sector mySector, IEnumerable<Sector> enemySectors)
        {
            var friendlyRating = mySector.GetFriendlyShips(faction).GetTotalRating();
            var enemyRating = enemySectors.Sum(sector => faction.GetEnemyRatingFor(sector));
            return friendlyRating/enemyRating;
        }

        public static decimal CombatSuccessRatio(this Faction faction, Sector sector)
        {
            return CombatSuccessRatio(faction, sector, new[] {sector});
        }

        public static decimal GetEnemyRatingFor(this Faction faction, Sector sector)
        {
            var rating = sector.GetEnemyShips(faction).GetTotalRating();
            return rating > 1 ? rating : 1;
        }

        public static IAction ChooseBest(this IEnumerable<ActionRating> actionRatings, decimal ratingMinimum, ILog log)
        {
            var possibleActions = actionRatings.Where(x => x.Action != null && x.Rating > ratingMinimum).ToList();
            if (!possibleActions.Any())
                return null;

            possibleActions.Each(x => log.Log("\t\t{0} [{1:n2}]", x.Action, x.Rating));
            var bestRating = possibleActions.Max(x => x.Rating);
            var bestAction = possibleActions.Where(x=>x.Rating==bestRating).PickRandom().Action;
            return bestAction;
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
            if (faction.Game.Round == 10)
            {
                return (faction.Material)/faction.TradeRatio + (faction.Science)/faction.TradeRatio;
            }
            if(faction.Sectors.Any(x=>x.GetEnemyShips(faction).Any()))
            {
                return (faction.Material - 5) / faction.TradeRatio + (faction.Science - 5) / faction.TradeRatio;
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

        public static bool IsEconomic(this Tech x)
        {
            return !IsMilitary(x);
        }

        public static bool IsMilitary(this Tech x)
        {
            return x is PartTech || Equals(x, Tech.NeutronBombs) || Equals(x, Tech.WormholeGenerator);
        }

        public static Tech PickBest(this IEnumerable<Tech> available, Faction faction)
        {
            var techs = available.ToArray();
            if (techs.Length == 0)
                return null;
            var highestLevel = (from tech in techs let maxCost = techs.Max(x => x.Cost) where tech.Cost == maxCost select tech).ToArray();
            var best = (from tech in highestLevel let minCost = highestLevel.Min(x => faction.CostFor(x)) where faction.CostFor(tech) == minCost select tech).ToArray();
            return best.PickRandom();
        }

        public static IAction DecideAction(this IEnumerable<IAiDecision> decisions, IAiPlayer player)
        {
            var state = new AiState(player.Faction);
            foreach (var decision in decisions)
            {
                var action = decision.Decide(state);
                if (action != null)
                    return action;
            }
            return player.GetAction<PassAction>();
        }

    }
}
