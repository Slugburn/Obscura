using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class SafeDecision : IActionDecision
    {
        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            if (player.GetAction<ResearchAction>() != null && faction.Science >= 8)
                return new ActionDecisionResult(new ResearchDecision());
            if (player.GetAction<BuildAction>() != null && faction.Material >= 13)
                return new ActionDecisionResult(new BuildDecision());
            if (player.GetAction<ExploreAction>() != null)
                return new ActionDecisionResult(new ExploreDecision());
            return new ActionDecisionResult(new AttackDecision());
        }
    }
}
