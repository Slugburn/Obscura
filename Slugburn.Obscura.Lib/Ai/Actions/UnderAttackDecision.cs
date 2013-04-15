using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class UnderAttackDecision : IActionDecision
    {
        private readonly UnderThreatDecision _underThreatDecision;
        private readonly DefendRallyPointDecision _defendRallyPointDecision;

        public UnderAttackDecision(UnderThreatDecision underThreatDecision, DefendRallyPointDecision defendRallyPointDecision)
        {
            _underThreatDecision = underThreatDecision;
            _defendRallyPointDecision = defendRallyPointDecision;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var sectorsUnderAttack = faction.Sectors.Where(s => s.GetEnemyShips(faction).Any());
            var mostNeedsDefending = (from sector in sectorsUnderAttack
                                      let ratio = faction.CombatSuccessRatio(sector)
                                      where ratio < 2
                                      orderby ratio
                                      select sector).FirstOrDefault();
            if (mostNeedsDefending == null)
                return new ActionDecisionResult(_underThreatDecision);
            player.RallyPoint = mostNeedsDefending;
            return new ActionDecisionResult(_defendRallyPointDecision);
        }
    }
}