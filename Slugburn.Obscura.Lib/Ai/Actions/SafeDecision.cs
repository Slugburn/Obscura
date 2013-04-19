using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Generators;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class SafeDecision : IActionDecision
    {
        private readonly ResearchListGenerator _researchListGenerator;
        private readonly EconomicResearchDecision _economicResearchDecision;
        private readonly BuildDecision _buildDecision;
        private readonly UnderThreatDecision _underThreatDecision;
        private readonly ILog _log;

        public SafeDecision(
            ResearchListGenerator researchListGenerator,
            EconomicResearchDecision economicResearchDecision,
            BuildDecision buildDecision, 
            UnderThreatDecision underThreatDecision,
            ILog log)
        {
            _researchListGenerator = researchListGenerator;
            _economicResearchDecision = economicResearchDecision;
            _buildDecision = buildDecision;
            _underThreatDecision = underThreatDecision;
            _log = log;
        }
        
        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            if (player.GetAction<ResearchAction>() != null)
            {
                var militaryResearchRating = _researchListGenerator.RateFleet(player);
                if (militaryResearchRating > 0)
                {
                    _log.Log("{0} decides to research {1}", faction, player.TechToResearch);
                    return new ActionDecisionResult(player.GetAction<ResearchAction>());
                }
                if (faction.AvailableResearchTech().Any(x => x.IsEconomic()))
                    return new ActionDecisionResult(_economicResearchDecision);
            }
            if (player.GetAction<InfluenceAction>() != null && faction.GetInfluencePlacementLocations().Any())
                return new ActionDecisionResult(new InfluenceDecision());
            if (player.GetAction<BuildAction>() != null && faction.Material >= 13)
                return new ActionDecisionResult(_buildDecision);
            return new ActionDecisionResult(_underThreatDecision);
        }
    }
}
