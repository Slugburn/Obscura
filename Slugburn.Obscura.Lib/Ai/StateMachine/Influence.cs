using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class Influence : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            var canInfluence = state.Faction.GetInfluencePlacementLocations().ToArray();
            if (!canInfluence.Any())
                return null;

            state.Faction.Colonize();

            state.Player.InfluenceList = canInfluence
                .OrderByDescending(x => x.GetSectorRating())
                .Take(2)
                .Select(x => new InfluenceLocation {Location = x})
                .ToList();
            return state.GetAction<InfluenceAction>();
        }
    }
}