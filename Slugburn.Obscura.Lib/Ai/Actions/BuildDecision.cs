using System;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class BuildDecision : IDecision<IAction>
    {
        private readonly BuildListGenerator _generator;
        private readonly ILog _log;

        public BuildDecision(BuildListGenerator generator, ILog log)
        {
            _generator = generator;
            _log = log;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var buildList = _generator.Generate(faction, faction.Sectors, BuildListGenerator.RateEconomicEfficiency);
            player.BuildList = buildList;
            var buildListDescription = string.Join(", ", buildList.Select(b => string.Format("{0} in {1}", b.Builder, b.Location)));
            _log.Log("{0} decides to build {1}", faction, buildListDescription);
            return new ActionDecisionResult(player.GetAction<BuildAction>());
        }
    }
}