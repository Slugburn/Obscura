using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ShouldPassDecision : IActionDecision
    {
        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            return player.Faction.SpendingInfluenceWillBankrupt()
                       ? new ActionDecisionResult(new PassAction())
                       : new ActionDecisionResult(new UnderAttack());
        }
    }
}
