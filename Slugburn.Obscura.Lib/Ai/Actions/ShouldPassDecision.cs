using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ShouldPassDecision : IActionDecision
    {
        private readonly UnderAttackDecision _underAttackDecision;

        public ShouldPassDecision(UnderAttackDecision underAttackDecision)
        {
            _underAttackDecision = underAttackDecision;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            return player.Faction.SpendingInfluenceWillBankrupt()
                       ? new ActionDecisionResult(player.GetAction<PassAction>())
                       : new ActionDecisionResult(_underAttackDecision);
        }
    }
}
