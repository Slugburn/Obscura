using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Generators;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class ImproveFleet : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            var player = state.Player;
            if (player.StagingPoint == null)
            {
                var mostNeedsDefending = UnderThreat.MostNeedsDefending(player.Faction);
                if (mostNeedsDefending == null)
                    return null;
                player.StagingPoint = mostNeedsDefending;
            }

            // possible actions are building ships, upgrading ships
            var actionRatings = new[]
                                    {
                                        new ActionRating(player.GetAction<BuildAction>(),
                                                         player.BuildListGenerator.RateStagingPoint(player, BuildListGenerator.RateAttackEfficency)),
                                        new ActionRating(player.GetAction<UpgradeAction>(), player.UpgradeListGenerator.RateFleet(player)),
                                    };
            return actionRatings.ChooseBest(player.ActionRatingMinimum, player.Log);
        }
    }
}
