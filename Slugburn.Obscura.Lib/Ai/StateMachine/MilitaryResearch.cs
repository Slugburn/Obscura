using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class MilitaryResearch : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            var faction = state.Faction;
            var milTech = faction.AvailableResearchTech().Where(tech => tech.IsMilitary()).ToArray();
            if (!milTech.Any())
                return null;

            var bestPartTechCost = faction.Technologies
                .Where(tech => tech is PartTech)
                .Cast<PartTech>()
                .GroupBy(tech => tech.PartType)
                .ToDictionary(g => g.Key, g => g.Max(tech => tech.Cost));
            var biggestImprovement = milTech
                .Where(tech => tech is PartTech)
                .Cast<PartTech>()
                .Select(tech => new {tech, rating = bestPartTechCost.ContainsKey(tech.PartType) ? tech.Cost - bestPartTechCost[tech.PartType] : tech.Cost})
                .Where(x => x.rating > 0)
                .OrderByDescending(x => x.rating)
                .Select(x => x.tech)
                .FirstOrDefault();

            state.Player.TechToResearch = biggestImprovement ?? milTech.OrderByDescending(tech => tech.Cost).FirstOrDefault();

            return faction.GetAction<ResearchAction>();
        }
    }
}