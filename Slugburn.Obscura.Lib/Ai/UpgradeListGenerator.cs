using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai
{
    public class UpgradeListGenerator
    {
        public IList<BlueprintUpgrade> Generate(IAiPlayer player)
        {
            var upgrades = GetAllPossibleUpgrades(player);
            var sets = GetPossibleUpgradeSets(player.Faction.UpgradeCount, upgrades);
            
            // Get the set with the best possible rating improvement
            return sets
                .OrderByDescending(set => set.Sum(x => x.RatingImprovement))
                .FirstOrDefault();
        }

        private static IEnumerable<List<BlueprintUpgrade>> GetPossibleUpgradeSets(int remainingPicks, List<BlueprintUpgrade> upgrades)
        {
            var sets = new List<List<BlueprintUpgrade>>();
            var validPicks = upgrades.Where(u => u.Order == 0);
            foreach (var upgrade in validPicks)
            {
                if (remainingPicks==1)
                {
                    sets.Add(new List<BlueprintUpgrade> {upgrade});
                }
                else
                {
                    var blueprintUpgrades = upgrades.Where(x => x.Blueprint == upgrade.Blueprint).ToArray();
                    var remainingForBlueprint = blueprintUpgrades.Where(x => x.Order > 0)
                        .Select(x => new BlueprintUpgrade {Blueprint = x.Blueprint, Upgrade = x.Upgrade, Replace = x.Replace, Order = x.Order - 1});
                    var remainingUpgrades = upgrades.Except(blueprintUpgrades).Concat(remainingForBlueprint).ToList();
                    var additional = GetPossibleUpgradeSets(remainingPicks - 1, remainingUpgrades);
                    sets.AddRange(additional.Select(additionalSet => new[] {upgrade}.Concat(additionalSet).ToList()));
                }
            }
            return sets;
        }

        private List<BlueprintUpgrade> GetAllPossibleUpgrades(IAiPlayer player)
        {
            var faction = player.Faction;
            var upgrades = new List<BlueprintUpgrade>();
            foreach (var blueprint in faction.Blueprints)
            {
                var shipCount = faction.Ships.Count(s => s.Blueprint == blueprint);
                var ideal = player.GetIdealPartList(blueprint);
                var current = blueprint.Parts;
                for (var i = 0; i < faction.UpgradeCount; i++)
                {
                    var upgrade = ChooseUpgrade(current, ideal);
                    if (upgrade == null) // current matches ideal
                        break;
                    var item = new BlueprintUpgrade {Blueprint = blueprint, Before = current.ToList(), After = current.ToList(), Upgrade = upgrade, Order = i};
                    item.After.Add(item.Upgrade);
                    if (current.Count == blueprint.PartSpaces)
                    {
                        item.Replace = ChoosePartToReplace(current, ideal);
                        item.After.Remove(item.Replace);
                    }
                    item.RatingImprovement = shipCount*(ShipProfile.Create(blueprint, item.After).Rating - ShipProfile.Create(blueprint, item.Before).Rating);
                    upgrades.Add(item);
                    current = item.After;
                }
            }
            return upgrades;
        }

        public ShipPart ChooseUpgrade(IEnumerable<ShipPart> current, IEnumerable<ShipPart> ideal)
        {
            // Get a part that is in the ideal template but not the current
            return ideal.Less(current)
                // make sure to upgrade to new energy sources first
                .OrderByDescending(x => x.Energy)
                .FirstOrDefault();
        }


        public ShipPart ChoosePartToReplace(IEnumerable<ShipPart> current, IEnumerable<ShipPart> ideal)
        {
            // Get a part that is not in the ideal template
            return current.Less(ideal)
                // replace drives last so that ship always has a drive
                .OrderBy(x => x.Move)
                .ThenBy(x => x.Energy)
                .First();
        }

    }
}