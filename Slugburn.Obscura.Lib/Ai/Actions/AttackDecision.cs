using System;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class AttackDecision : IActionDecision
    {
        private readonly AssaultRallyPointDecision _assaultRallyPointDecision;
        private readonly ImproveFleetDecision _improveFleetDecision;
        private readonly ExploreDecision _exploreDecision;
        private readonly ILog _log;

        public AttackDecision(AssaultRallyPointDecision assaultRallyPointDecision, ImproveFleetDecision improveFleetDecision, ExploreDecision exploreDecision, ILog log)
        {
            _assaultRallyPointDecision = assaultRallyPointDecision;
            _improveFleetDecision = improveFleetDecision;
            _exploreDecision = exploreDecision;
            _log = log;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var targets = faction.Game.Map.GetSectors().Where(s => s.Owner != faction & faction.Sectors.Any(x=>faction.GetShortestPath(x,s) != null)).ToArray();
            if (targets.Length == 0)
                return new ActionDecisionResult(_exploreDecision);

            var conquerable = targets.Where(faction.FleetCanConquer).ToArray();
            if (conquerable.Any())
            {
                // rank sectors according to proximity and value
                var target = conquerable.OrderByDescending(faction.GetEnemySectorRating).First();
                SetTarget(player, target);
                _log.Log("{0} decides to conquer {1}", faction, target);
                return new ActionDecisionResult(_assaultRallyPointDecision);
            }
            else
            {
                var target = targets.OrderByDescending(faction.GetEnemySectorRating).First();
                SetTarget(player, target);
                _log.Log("{0} decides to begin staging forces at {1} to attack {2}", faction, player.StagingPoint, target);
                return new ActionDecisionResult(_improveFleetDecision);
            }
        }

        private static void SetTarget(IAiPlayer player, Sector target)
        {
            player.ThreatPoint = target;
            player.RallyPoint = target;
            var stagingPoint = player.Faction.GetClosestSectorTo(target);
            if (stagingPoint == null)
                throw new InvalidOperationException(string.Format("No path found from any of {0}'s sectors to {1}", player.Faction, target));
            player.StagingPoint = stagingPoint;
        }
    }
}