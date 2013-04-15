using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class SafeDecision : IActionDecision
    {
        private readonly ResearchDecision _researchDecision;
        private readonly BuildDecision _buildDecision;
        private readonly ExploreDecision _exploreDecision;
        private readonly AttackDecision _attackDecision;

        public SafeDecision(ResearchDecision researchDecision, BuildDecision buildDecision, ExploreDecision exploreDecision, AttackDecision attackDecision)
        {
            _researchDecision = researchDecision;
            _buildDecision = buildDecision;
            _exploreDecision = exploreDecision;
            _attackDecision = attackDecision;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            if (player.GetAction<ResearchAction>() != null && faction.Science >= 8)
                return new ActionDecisionResult(_researchDecision);
            if (player.GetAction<BuildAction>() != null && faction.Material >= 13)
                return new ActionDecisionResult(_buildDecision);
            return new ActionDecisionResult(_attackDecision);
            if (player.GetAction<ExploreAction>() != null)
                return new ActionDecisionResult(_exploreDecision);
        }
    }
}
