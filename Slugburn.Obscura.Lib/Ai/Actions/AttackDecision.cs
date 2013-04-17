using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class AttackDecision : IActionDecision
    {
        private readonly AssaultRallyPointDecision _assaultRallyPointDecision;
        private readonly ImproveFleetDecision _improveFleetDecision;
        private readonly ILog _log;

        public AttackDecision(AssaultRallyPointDecision assaultRallyPointDecision, ImproveFleetDecision improveFleetDecision, ILog log)
        {
            _assaultRallyPointDecision = assaultRallyPointDecision;
            _improveFleetDecision = improveFleetDecision;
            _log = log;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var targets = faction.Game.Map.GetSectors().Where(s => s.Owner != faction).ToArray();
            if (targets.Length == 0)
                return new ActionDecisionResult(new PassAction());

            var conquerable = targets.Where(faction.FleetCanConquer).ToArray();
            if (conquerable.Any())
            {
                // rank sectors according to proximity and value
                var target = conquerable.OrderByDescending(faction.GetEnemySectorRating).First();
                player.ThreatPoint = target;
                player.RallyPoint = target;
                _log.Log("{0} decides to conquer {1}", faction, target);
                return new ActionDecisionResult(_assaultRallyPointDecision);
            }
            else
            {
                var target = targets.OrderByDescending(faction.GetEnemySectorRating).First();
                player.StagingPoint = target;
                return new ActionDecisionResult(_improveFleetDecision);
            }
        }
    }
}