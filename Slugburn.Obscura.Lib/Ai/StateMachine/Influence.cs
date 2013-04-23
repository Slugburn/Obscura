using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class Influence : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            var player = state.Player;
            var faction = state.Faction;

            var canInfluence = faction.GetInfluencePlacementLocations().ToArray();
            if (!canInfluence.Any())
                return null;

            player.InfluenceList = canInfluence
                .OrderByDescending(x => x.GetSectorRating())
                .Take(2)
                .Select(x => new InfluenceLocation {Location = x})
                .ToList();

            if (player.GetActionsBeforeBankruptcy() < 1 + player.InfluenceList.Count)
                return null;

            faction.Colonize();

            return state.GetAction<InfluenceAction>();
        }
    }
}