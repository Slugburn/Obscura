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
                var upgrade = faction.Player.ChooseUpgrade(blueprint);
                if (replace == null)
                    _log.Log("\t{0} upgrades {1} with {2}", faction, blueprint, upgrade);
                else
                    _log.Log("\t{0} upgrades {1} with {2}, replacing {3}", faction, blueprint, upgrade, replace);
                blueprint.Upgrade(upgrade, replace);
                upgradesCompleted++;
                _log.Log("\t{0} {1}: {2}", faction, blueprint, blueprint.Parts.ListToString());
                faction.Player.AfterUpgradeCompleted();
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

        public bool IsValid(PlayerFaction faction)
        {
            return !faction.Passed && faction.Influence > 0;
        }
    }
}