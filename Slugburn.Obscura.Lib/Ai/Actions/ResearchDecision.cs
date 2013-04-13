using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Extensions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ResearchDecision : IDecision<IAction>
    {
        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var techs = faction.Game.AvailableTechTiles.Except(faction.Technologies);
            var canResearch = (from tech in techs where faction.CostFor(tech) <= faction.Science select tech).ToArray();
            var highestLevel = (from tech in canResearch let maxCost = canResearch.Max(x => x.Cost) where tech.Cost == maxCost select tech).ToArray();
            var best = from tech in highestLevel let minCost = highestLevel.Min(x=>faction.CostFor(x)) where faction.CostFor(tech) == minCost select tech;
            player.TechToResearch = best.PickRandom();
            return new ActionDecisionResult(player.GetAction<ResearchAction>());
        }
    }
}