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

        public string Name { get { return "Upgrade"; } }

        public void Do(Faction faction)
        {
            var upgradesCompleted = 0;
            while (upgradesCompleted < faction.UpgradeCount)
            {
                var blueprint = faction.Player.ChooseBlueprintToUpgrade(faction.Blueprints);
                if (blueprint == null)
                    break;
                var availableParts = faction.GetAvailableShipParts();
                ShipPart replace = null;
                if (blueprint.Parts.Count == blueprint.PartSpaces)
                    replace = faction.Player.ChoosePartToReplace(blueprint);
                var validUpgrades = availableParts.Where(part => IsUpgradeValid(blueprint, part, replace));
                var upgrade = faction.Player.ChooseUpgrade(blueprint, validUpgrades);
                if (replace == null)
                    _log.Log("{0} upgrades {1} with {2}", faction.Name, blueprint.Name, upgrade.Name);
                else
                    _log.Log("{0} upgrades {1} with {2}, replacing {3}", faction.Name, blueprint.Name, upgrade.Name, replace.Name);
                blueprint.Upgrade(upgrade, replace);
                upgradesCompleted++;
                if (string.IsNullOrWhiteSpace(upgrade.Name))
                    throw new Exception("Part of type {0} does not have a name");
            }
        }

        private bool IsUpgradeValid(ShipBlueprint blueprint, ShipPart part, ShipPart replace)
        {
            var newParts = blueprint.Parts.Less(replace).ToList();
            newParts.Add(part);
            return blueprint.IsPartListValid(newParts);
        }

        public bool IsValid(Faction faction)
        {
            return !faction.Passed && faction.Influence > 0;
        }
    }
}