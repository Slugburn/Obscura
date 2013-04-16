using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class AttackDecision : IActionDecision
    {
        private readonly AttackRallyPointDecision _attackRallyPointDecision;
        private readonly ILog _log;

        public AttackDecision(AttackRallyPointDecision attackRallyPointDecision, ILog log)
        {
            _attackRallyPointDecision = attackRallyPointDecision;
            _log = log;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var ancientSector = GetBestAncientSector(faction);
            if (ancientSector != null)
            {
                _log.Log("{0} decides to attack Ancients in {1}", faction, ancientSector);
                player.RallyPoint = ancientSector;
                return new ActionDecisionResult(_attackRallyPointDecision);
            }

            var galacticCore = faction.Game.GalacticCore;
            var pathToCore = MoveListGenerator.GetPath(faction, faction.HomeSector, galacticCore);
            if (pathToCore != null && pathToCore.Count > 0)
            {
                _log.Log("{0} decides to attack Galactice Core", faction);
                player.RallyPoint = galacticCore;
                return new ActionDecisionResult(_attackRallyPointDecision);
            }

            return new ActionDecisionResult(new ExploreDecision());
        }

        private Sector GetBestAncientSector(PlayerFaction faction)
        {
            // for now, just look for ancient sectors that are adjacent to our sectors
            var sectors = faction.Sectors
                .SelectMany(s => s.AdjacentSectors()
                    .Where(x => x.Ships.Any(ship => ship is AncientShip) && !x.GetFriendlyShips(faction).Any()))
                    .Distinct().ToArray();
            return sectors.Any() ? sectors.PickRandom() : null;
        }
    }
}