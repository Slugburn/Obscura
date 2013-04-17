using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class UnderAttackDecision : IActionDecision
    {
        private readonly UnderThreatDecision _underThreatDecision;
        private readonly AssaultRallyPointDecision _assaultRallyPointDecision;

        public UnderAttackDecision(UnderThreatDecision underThreatDecision, AssaultRallyPointDecision assaultRallyPointDecision)
        {
            _underThreatDecision = underThreatDecision;
            _assaultRallyPointDecision = assaultRallyPointDecision;
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
                return new ActionDecisionResult(_underThreatDecision);
            player.ThreatPoint = mostNeedsDefending;
            player.RallyPoint = mostNeedsDefending;
            player.StagingPoint = faction.GetClosestSectorTo(mostNeedsDefending);
            return new ActionDecisionResult(_assaultRallyPointDecision);
        }
    }
}
