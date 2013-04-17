using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class SafeDecision : IActionDecision
    {
        private readonly EconomicResearchDecision _economicResearchDecision;
        private readonly BuildDecision _buildDecision;
        private readonly ExploreDecision _exploreDecision;
        private readonly AttackDecision _attackDecision;

        public SafeDecision(
            EconomicResearchDecision economicResearchDecision, 
            BuildDecision buildDecision, 
            ExploreDecision exploreDecision, 
            AttackDecision attackDecision)
        {
            _economicResearchDecision = economicResearchDecision;
            _buildDecision = buildDecision;
            _exploreDecision = exploreDecision;
            _attackDecision = attackDecision;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            if (player.GetAction<ResearchAction>() != null && faction.AvailableResearchTech().Any(x=>!(x is PartTech)))
                return new ActionDecisionResult(_economicResearchDecision);
            if (player.GetAction<InfluenceAction>() != null && faction.GetInfluencePlacementLocations().Any())
                return new ActionDecisionResult(new InfluenceDecision());
            if (player.GetAction<BuildAction>() != null && faction.Material >= 13)
                return new ActionDecisionResult(_buildDecision);
            if (player.GetAction<ExploreAction>() != null)
                return new ActionDecisionResult(_exploreDecision);
            return new ActionDecisionResult(_attackDecision);
        }
    }
}
