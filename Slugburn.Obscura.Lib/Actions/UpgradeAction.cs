using System;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Actions
{
    public class UpgradeAction : IAction
    {
        private readonly ILog _log;

        public UpgradeAction(ILog log)
        {
            _log = log;
        }

        public override string ToString()
        {
            return "Upgrade";
        }

        public void Do(PlayerFaction faction)
        {
            Upgrade(faction, faction.UpgradeCount);
        }

        private void Upgrade(PlayerFaction faction, int upgradeCount)
        {
            var upgradesCompleted = 0;
            while (upgradesCompleted < upgradeCount)
            {
                if (Upgrade(faction)) break;
                upgradesCompleted++;
                faction.Player.AfterUpgradeCompleted();
            }
        }

        private bool Upgrade(PlayerFaction faction)
        {
            var blueprint = faction.Player.ChooseBlueprintToUpgrade(faction.Blueprints);
            if (blueprint == null)
                return true;
            var availableParts = faction.GetAvailableShipParts().ToArray();
            ShipPart replace = null;
            replace = faction.Player.ChoosePartToReplace(blueprint, availableParts);
            var validUpgrades = availableParts.Where(part => IsUpgradeValid(blueprint, part, replace)).ToArray();
            var upgrade = faction.Player.ChooseUpgrade(blueprint, validUpgrades);
            if (string.IsNullOrWhiteSpace(upgrade.Name))
                throw new Exception("Part of type {0} does not have a name");
            if (validUpgrades.All(x=> !Equals(upgrade, x)))
                throw new InvalidOperationException(string.Format("{0} is not a valid upgrade part", upgrade));
            
            if (replace == null)
                _log.Log("\t{0} upgrades {1} with {2}", faction, blueprint, upgrade);
            else
                _log.Log("\t{0} upgrades {1} with {2}, replacing {3}", faction, blueprint, upgrade, replace);

            blueprint.Upgrade(upgrade, replace);
            _log.Log("\t{0} {1}: {2}", faction, blueprint, blueprint.Parts.ListToString());
            return false;
        }

        private bool IsUpgradeValid(ShipBlueprint blueprint, ShipPart part, ShipPart replace)
        {
            var newParts = blueprint.Parts.Less(replace).ToList();
            newParts.Add(part);
            return blueprint.IsPartListValid(newParts);
        }

        public bool IsValid(PlayerFaction faction)
        {
            return faction.Influence > 0;
        }

        public void UpgradeUsingDiscoveredPart(PlayerFaction faction, ShipPart upgrade)
        {
            // TODO: Integrate with main upgrade logic
            faction.DiscoveredParts.Add(upgrade);
            faction.Player.BeforeUpgradeWithDiscoveredPart(upgrade);

            var upgradeableBlueprints = faction.Blueprints.Where(bp => bp.CanUsePartToUpgrade(upgrade)).ToArray();
            if (!upgradeableBlueprints.Any())
                return;
            
            var blueprint = faction.Player.ChooseBlueprintToUpgradeWithDiscoveredPart(upgradeableBlueprints);
            if (blueprint == null)
                return;
            if (upgradeableBlueprints.All(x => blueprint != x))
                throw new InvalidOperationException(string.Format("{0} can not be used to upgrade {1}", upgrade, blueprint));

            var validReplacements = blueprint.GetValidPartsToReplace(upgrade).ToArray();
            var replace = faction.Player.ChoosePartToReplace(blueprint, validReplacements);
            if (validReplacements.All(x=>!(replace==null && x==null || replace != null && replace.Equals(x))))
                throw new InvalidOperationException(string.Format("{0}: {1} can not be replaced with {2}", blueprint, replace, upgrade));

            blueprint.Upgrade(upgrade, replace);
            faction.DiscoveredParts.Remove(upgrade);

            _log.Log("\t{0} {1}: {2}", faction, blueprint, blueprint.Parts.ListToString());
        }
    }
}