using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Actions
{
    public class UpgradeAction : IAction
    {
        public UpgradeAction()
        {
        }

        public void Do(Faction faction)
        {
            var upgradesCompleted = 0;
            while (upgradesCompleted < faction.UpgradeCount)
            {
                var blueprint = faction.Player.ChooseBlueprintToUpgrade(faction.Blueprints);
                var availableParts = faction.GetAvailableShipParts();
                ShipPart replace = null;
                if (blueprint.Parts.Count == blueprint.PartSpaces)
                    replace = faction.Player.ChoosePartToReplace(blueprint);
                var validUpgrades = availableParts.Where(part => IsUpgradeValid(blueprint, part, replace));
                var upgrade = faction.Player.ChooseUpgrade(validUpgrades);
                blueprint.Upgrade(upgrade, replace);
                upgradesCompleted++;
            }
        }

        private bool IsUpgradeValid(ShipBlueprint blueprint, ShipPart part, ShipPart replace)
        {
            var newParts = blueprint.Parts.Except(new[]{replace}).ToList();
            newParts.Add(part);
            return blueprint.IsPartListValid(newParts);
        }

        public bool IsValid(Faction faction)
        {
            return true;
        }
    }
}