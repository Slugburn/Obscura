using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class BuildDecision : IDecision<IAction>
    {
        private readonly BuildListGenerator _generator;
        private readonly ILog _log;
        private readonly ExploreDecision _exploreDecision;

        public BuildDecision(BuildListGenerator generator, ILog log, ExploreDecision exploreDecision)
        {
            _generator = generator;
            _log = log;
            _exploreDecision = exploreDecision;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            if (faction.HasTechnology(Tech.Orbital) || faction.HasTechnology(Tech.Monolith))
            {
                var buildList = _generator.Generate(faction, faction.Sectors, BuildListGenerator.RateEconomicEfficiency);
                if (buildList != null)
                {
                    player.BuildList = buildList;
                    var buildListDescription = string.Join(", ", buildList.Select(b => string.Format("{0} in {1}", b.Builder, b.Location)));
                    _log.Log("{0} decides to build {1}", faction, buildListDescription);
                    return new ActionDecisionResult(player.GetAction<BuildAction>());
                }
            }
            return new ActionDecisionResult(_exploreDecision);
        }
    }
}