using System;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ExploreDecision : IActionDecision
    {
        private readonly AttackDecision _attackDecision;

        public ExploreDecision(AttackDecision attackDecision)
        {
            _attackDecision = attackDecision;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var explore = player.GetAction<ExploreAction>();
            if (explore != null)
                return new ActionDecisionResult(explore);
            return new ActionDecisionResult(_attackDecision);
        }
    }
}
