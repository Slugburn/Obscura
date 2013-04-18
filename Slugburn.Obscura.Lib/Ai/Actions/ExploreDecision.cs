using System;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ExploreDecision : IActionDecision
    {
        public ExploreDecision()
        {
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var explore = player.GetAction<ExploreAction>();
            if (explore != null)
                return new ActionDecisionResult(explore);
            return new ActionDecisionResult(player.GetAction<PassAction>());
        }
    }
}
