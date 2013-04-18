using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class UnderAttackDecision : IActionDecision
    {
        private readonly SafeDecision _safeDecision;
        private readonly AssaultRallyPointDecision _assaultRallyPointDecision;
        private readonly DefendOwnSectorDecision _defendOwnSectorDecision;
        private readonly ILog _log;

        public UnderAttackDecision(SafeDecision safeDecision, AssaultRallyPointDecision assaultRallyPointDecision, DefendOwnSectorDecision defendOwnSectorDecision, ILog log)
        {
            _safeDecision = safeDecision;
            _assaultRallyPointDecision = assaultRallyPointDecision;
            _defendOwnSectorDecision = defendOwnSectorDecision;
            _log = log;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var sectors = faction.Sectors.Concat(faction.Ships.Select(x=>x.Sector)).Distinct();
            var sectorsUnderAttack = sectors.Where(s => s.GetEnemyShips(faction).Any());
            var mostNeedsDefending = (from sector in sectorsUnderAttack
                                      let ratio = faction.CombatSuccessRatio(sector)
                                      where ratio < 2
                                      orderby ratio
                                      select sector).FirstOrDefault();
            if (mostNeedsDefending == null)
                return new ActionDecisionResult(_safeDecision);
            
            player.ThreatPoint = mostNeedsDefending;
            player.RallyPoint = mostNeedsDefending;
            player.StagingPoint = faction.GetClosestSectorTo(mostNeedsDefending);

            if (player.ThreatPoint.Owner == player.Faction)
            {
                _log.Log("{0} decides to defend {1} from attack by {2}", faction, mostNeedsDefending, mostNeedsDefending.GetEnemyShips(faction).First().Faction);
                return new ActionDecisionResult(_defendOwnSectorDecision);
            }

            _log.Log("{0} decides to continue assault on {1}", faction, mostNeedsDefending);
            return new ActionDecisionResult(_assaultRallyPointDecision);
        }
    }
}
