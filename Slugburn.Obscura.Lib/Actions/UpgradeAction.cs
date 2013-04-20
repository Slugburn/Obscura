using System;
using System.Linq;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Messages;
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

        public void Do(Faction faction)
        {
            var upgradesCompleted = 0;
            while (upgradesCompleted < faction.UpgradeCount)
            {
                faction.SendMessage(new BeforeUpgrade());
                Upgrade(faction, faction.GetAvailableShipParts().ToArray());
                upgradesCompleted++;
            }
        }

        public void UpgradeToDiscoveredPart(Faction faction, ShipPart part)
        {
            faction.SendMessage(new BeforeUpgradeToDiscoveredPart(part));
            Upgrade(faction, new[] {part} );
        }

        public bool IsValid(Faction faction)
        {
            return faction.Influence > 0;
        }

        public void Upgrade(Faction faction, ShipPart[] partsPool)
        {
            ShipBlueprint blueprint = null;
            try
            {
                var upgradeableBlueprints = faction.Blueprints.Where(bp => partsPool.Any(bp.CanUsePartToUpgrade)).ToArray();
                if (!upgradeableBlueprints.Any())
                    return;
            
                blueprint = faction.Player.ChooseBlueprintToUpgrade(upgradeableBlueprints);
                if (blueprint == null)
                    return;
                if (upgradeableBlueprints.All(x => blueprint != x))
                    throw new InvalidOperationException(string.Format("{0} cannot be upgraded with any available parts", blueprint));

                var validUpgrades = partsPool.Where(blueprint.CanUsePartToUpgrade).ToArray();
                var upgradeAlreadyChosen = partsPool.Length == 1;
                var upgrade = upgradeAlreadyChosen ? partsPool[0] : faction.Player.ChooseUpgrade(blueprint, validUpgrades);
                if (upgrade == null)
                    return;
                if (validUpgrades.All(x => !Equals(upgrade, x)))
                    throw new InvalidOperationException(string.Format("{0} can not be upgraded with {1}", blueprint, upgrade));

                var validReplacements = blueprint.GetValidPartsToReplace(upgrade).ToArray();
                var replace = faction.Player.ChoosePartToReplace(blueprint, validReplacements);
                if (validReplacements.All(x=>!(replace==null && x==null || replace != null && replace.Equals(x))))
                {
                    if (replace == null)
                        throw new InvalidOperationException(string.Format("{0}: {1} can not be added", blueprint, upgrade));
                    throw new InvalidOperationException(string.Format("{0}: {1} can not be replaced with {2}", blueprint, replace, upgrade));
                }

                if (replace == null)
                    _log.Log("\t{0} upgrades {1} with {2}", faction, blueprint, upgrade);
                else
                    _log.Log("\t{0} upgrades {1} with {2}, replacing {3}", faction, blueprint, upgrade, replace);

                blueprint.Upgrade(upgrade, replace);

                if (upgrade.IsUnique)
                {
                    faction.DiscoveredParts.Remove(upgrade);
                    faction.SendMessage(new PartListChanged());
                }

                faction.SendMessage(new UpgradeComplete());

                _log.Log("\t{0} {1}: {2}", faction, blueprint, blueprint.Parts.ListToString());
            }
            catch (Exception ex)
            {
                var aiPlayer = faction.Player as AiPlayer;
                if (aiPlayer != null && blueprint != null)
                {
                    _log.Log("Current: {0}", blueprint.Parts.ListToString());
                    _log.Log("Ideal  : {0}", aiPlayer.GetIdealPartList(blueprint).ListToString());
                    _log.Log("Upgrade: {0}", aiPlayer.UpgradeList[0].Upgrade);
                    _log.Log("Replace: {0}", aiPlayer.UpgradeList[0].Replace);
                }

                throw;
            }            
        }
    }
}