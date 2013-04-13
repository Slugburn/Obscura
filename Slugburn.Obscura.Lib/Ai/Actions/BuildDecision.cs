using System;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class BuildDecision : IDecision<IAction>
    {
        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            player.BuildList = player.GetGeneralPurposeBuildList();
            return new ActionDecisionResult(player.GetAction<BuildAction>());
        }
    }
}