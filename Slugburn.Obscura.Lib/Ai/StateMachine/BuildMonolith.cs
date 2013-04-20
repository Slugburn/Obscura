using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Generators;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class BuildMonolith : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            var faction = state.Faction;
            var player = state.Player;
            if (!faction.HasTechnology(Tech.Monolith)) 
                return null;
            var rating = player.BuildListGenerator.RateAllSectors(player, BuildListGenerator.RateEconomicEfficiency);
            return rating > 0 ? player.GetAction<BuildAction>() : null;
        }
    }
}