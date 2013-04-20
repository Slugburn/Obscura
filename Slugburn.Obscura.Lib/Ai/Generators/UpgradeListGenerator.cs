using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.Generators
{
    public class UpgradeListGenerator
    {
        public IList<BlueprintUpgrade> Generate(IAiPlayer player, IList<PlayerShip> shipsToUpgrade, int upgradeCount)
        {
            return _Generate(player, shipsToUpgrade, upgradeCount, player.Faction.GetAvailableShipParts());
        }

        public IList<BlueprintUpgrade> GenerateForDiscoveredPart(IAiPlayer player, ShipPart discoveredPart)
        {
            return _Generate(player, player.Faction.Ships, 1, new[] {discoveredPart});
        }

        private IList<BlueprintUpgrade> _Generate(IAiPlayer player, IList<PlayerShip> shipsToUpgrade, int upgradeCount, IEnumerable<ShipPart> availableUpgrades)
        {
            var faction = player.Faction;
            var blueprintsToUpgrade = new List<ShipBlueprint> {faction.Interceptor, faction.Cruiser, faction.Dreadnought};
            if (faction.HasTechnology(Tech.Starbase) && player.ThreatPoint != null && player.ThreatPoint.Owner == faction)
                blueprintsToUpgrade.Add(faction.Starbase);

            var upgrades = GetAllPossibleUpgrades(player, shipsToUpgrade, blueprintsToUpgrade)
                .Where(x => availableUpgrades.Contains(x.Upgrade))
                .ToList();
            var sets = GetPossibleUpgradeSets(upgradeCount, upgrades);

            // Get the set with the best possible rating improvement
            return sets
                .OrderByDescending(set => set.Sum(x => x.RatingImprovement))
                .FirstOrDefault();
        }

        private IEnumerable<BlueprintUpgrade> GetAllPossibleUpgrades(IAiPlayer player, IList<PlayerShip> shipsToUpgrade, IEnumerable<ShipBlueprint> blueprintsToUpgrade)
        {
            var upgrades = new List<BlueprintUpgrade>();
            foreach (var blueprint in blueprintsToUpgrade)
            {
                var shipCount = shipsToUpgrade.Count(s => s.Blueprint == blueprint);
                var ideal = player.GetIdealPartList(blueprint);
                var current = blueprint.Parts;
                for (var i = 0; i < player.Faction.UpgradeCount; i++)
                {
                    var upgrade = ChooseUpgrade(current, ideal);
                    if (upgrade == null) // current matches ideal
                        break;
                    var item = new BlueprintUpgrade { Blueprint = blueprint, Before = current.ToList(), After = current.ToList(), Upgrade = upgrade, Order = i };
                    item.After.Add(item.Upgrade);
                    if (item.After.Count > blueprint.PartSpaces)
                    {
                        item.Replace = ChoosePartToReplace(current, ideal);
                        if (item.Replace != null)
                            item.After.Remove(item.Replace);
                    }
                    item.RatingImprovement = (shipCount + 0.1m) * (ShipProfile.Create(blueprint, item.After).Rating - ShipProfile.Create(blueprint, item.Before).Rating);
                    upgrades.Add(item);
                    current = item.After;
                }
            }
            return upgrades;
        }

        private static ShipPart ChooseUpgrade(IEnumerable<ShipPart> current, IEnumerable<ShipPart> ideal)
        {
            // Get a part that is in the ideal template but not the current
            return ideal.Less(current)
                // make sure to upgrade to new energy sources first
                .OrderByDescending(x => x.Energy)
                .FirstOrDefault();
        }


        private static ShipPart ChoosePartToReplace(IEnumerable<ShipPart> current, IEnumerable<ShipPart> ideal)
        {
            // Get a part that is not in the ideal template
            return current.Less(ideal)
                // replace drives last so that ship always has a drive
                .OrderBy(x => x.Move)
                .ThenBy(x => x.Energy)
                .FirstOrDefault();
        }

        private static IEnumerable<List<BlueprintUpgrade>> GetPossibleUpgradeSets(int remainingPicks, List<BlueprintUpgrade> upgrades)
        {
            var sets = new List<List<BlueprintUpgrade>>();
            var validPicks = upgrades.Where(u => u.Order == 0);
            foreach (var upgrade in validPicks)
            {
                if (remainingPicks == 1)
                {
                    sets.Add(new List<BlueprintUpgrade> { upgrade });
                }
                else
                {
                    var blueprintUpgrades = upgrades.Where(x => x.Blueprint == upgrade.Blueprint).ToArray();
                    var remainingForBlueprint = blueprintUpgrades.Where(x => x.Order > 0)
                        .Select(x => new BlueprintUpgrade { Blueprint = x.Blueprint, Upgrade = x.Upgrade, Replace = x.Replace, Order = x.Order - 1 });
                    var remainingUpgrades = upgrades.Except(blueprintUpgrades).Concat(remainingForBlueprint).ToList();
                    var additional = GetPossibleUpgradeSets(remainingPicks - 1, remainingUpgrades);
                    var localUpgrade = upgrade;
                    sets.AddRange(additional.Select(additionalSet => new[] { localUpgrade }.Concat(additionalSet).ToList()));
                }
            }
            return sets;
        }

        public decimal RateRallyPoint(IAiPlayer player)
        {
            return RateRallyPoint(player, player.Faction.UpgradeCount);
        }

        public decimal RateRallyPoint(IAiPlayer player, int upgradeCount)
        {
            return _Rate(player, player.Faction.UpgradeCount, player.Faction.Ships.ToArray());
            //            return _Rate(player, upgradeCount, player.RallyPoint.GetFriendlyShips(player.Faction).ToArray());
        }

        public decimal RateFleet(IAiPlayer player)
        {
            return _Rate(player, player.Faction.UpgradeCount, player.Faction.Ships.ToArray());
        }

        private decimal _Rate(IAiPlayer player, int upgradeCount, PlayerShip[] ships)
        {
            if (player.GetAction<UpgradeAction>() == null)
                return 0;
            if (!ships.Any())
                return 0;
            player.UpgradeList = Generate(player, ships, upgradeCount);
            if (player.UpgradeList == null)
                return 0;
            return player.UpgradeList.Sum(x => x.RatingImprovement);
        }

        public decimal RateForMove(IAiPlayer player, MoveListGenerator moveListGenerator)
        {
            var faction = player.Faction;
            if (player.GetAction<UpgradeAction>() == null)
                return 0;
            var shipsAtRallyPoint = player.RallyPoint.GetFriendlyShips(faction);
            // TODO: Take upgraded moves into consideration
            var shipsAfterMove = moveListGenerator.Generate(player).Where(x => x.Moves.Last() == player.RallyPoint);
            var ships = shipsAtRallyPoint.Concat(shipsAfterMove.Select(x=>x.Ship)).ToArray();
            if (!ships.Any())
                return 0;
            player.UpgradeList = Generate(player, ships, faction.UpgradeCount);
            if (player.UpgradeList == null)
                return 0;
            return player.UpgradeList.Sum(x => x.RatingImprovement);
        }

    }
}