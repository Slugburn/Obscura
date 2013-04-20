using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class EconomicResearch : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            var econTech = state.Faction.AvailableResearchTech().Where(tech => AiExtensions.IsEconomic(tech)).ToArray();
            if (!econTech.Any())
                return null;
            state.Player.TechToResearch = econTech.PickBest(state.Faction);
            return state.Faction.GetAction<ResearchAction>();
        }
    }
}