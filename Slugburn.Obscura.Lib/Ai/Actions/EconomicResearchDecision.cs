using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class EconomicResearchDecision : IDecision<IAction>
    {
        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var techs = faction.AvailableResearchTech().Where(x=>!(x is PartTech));
            var canResearch = (from tech in techs where faction.CostFor(tech) <= faction.Science select tech);
            var picked = PickBestAvailableTech(faction, canResearch);
            player.TechToResearch = picked;
            return new ActionDecisionResult(player.GetAction<ResearchAction>());
        }

        public static Tech PickBestAvailableTech(PlayerFaction faction, IEnumerable<Tech> available)
        {
            var techs = available.ToArray();
            if (techs.Length == 0)
                return null;
            var highestLevel = (from tech in techs let maxCost = techs.Max(x => x.Cost) where tech.Cost == maxCost select tech).ToArray();
            var best = (from tech in highestLevel let minCost = highestLevel.Min(x => faction.CostFor(x)) where faction.CostFor(tech) == minCost select tech).ToArray();
            return best.PickRandom();
        }
    }
}