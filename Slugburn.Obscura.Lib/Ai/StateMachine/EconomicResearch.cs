using System;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class EconomicResearch : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            if (state.SavingForTech)
                return null;

            var faction = state.Faction;
            var maxToSpend = Math.Max(faction.Science, state.RoundSpendingLimit);

            var econTech = state.Faction.UnknownTech()
                .Where(tech => tech.IsEconomic() && faction.CostFor(tech) <= maxToSpend).ToArray();
            if (!econTech.Any())
                return null;

            var bestTech = econTech.PickBest(state.Faction);

            return state.ResearchOrSaveFor(bestTech);
        }
    }
}