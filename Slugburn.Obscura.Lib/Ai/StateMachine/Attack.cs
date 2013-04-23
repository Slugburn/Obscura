using System;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Generators;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class Attack : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            var faction = state.Faction;
            var player = state.Player;

            var targets = faction.Game.Map.GetSectors().Where(s => s.Owner != faction & faction.Sectors.Any(x => faction.GetShortestPath(x, s) != null)).ToArray();
            if (targets.Length == 0)
                return null;

            // if any targets are ancients, then ignore players
            var neutralTargets = targets.Where(x => x.Ships.Any(s => s.Owner is Ancients || s is GalacticCoreDefenseSystem)).ToArray();
            if (neutralTargets.Any())
                targets = neutralTargets;

            var conquerable = targets.Where(faction.FleetCanConquer).ToArray();
            if (conquerable.Any())
            {
                // rank sectors according to proximity and value
                var target = conquerable.OrderByDescending(faction.GetEnemySectorRating).First();
                SetTarget(player, target);
                player.Log.Log("{0} decides to conquer {1}", faction, target);

                return ChooseBestAction(player);
            }
            else
            {
                var target = targets.OrderByDescending(faction.GetEnemySectorRating).First();
                SetTarget(player, target);
                player.Log.Log("{0} decides to begin staging forces at {1} to attack {2}", faction, player.StagingPoint, target);
                return null;
            }


        }

        public static IAction ChooseBestAction(AiPlayer player)
        {
            var faction = player.Faction;
            if (player.GetActionsBeforeBankruptcy() < 2)
            {
                var oneActionRatings = new[]
                                           {
                                               new ActionRating(player.GetAction<MoveAction>(), player.MoveListGenerator.Rate(player)),
                                               new ActionRating(player.GetAction<UpgradeAction>(), player.UpgradeListGenerator.RateRallyPoint(player)),
                                           };
                return oneActionRatings.ChooseBest(player.ActionRatingMinimum, player.Log);
            }

            var twoActionRatings
                = new[]
                    {
                        // Move + Move
                        new ActionRating(player.GetAction<MoveAction>(), player.MoveListGenerator.Rate(player, faction.MoveCount*2)),

                        // Upgrade + Upgrade
                        new ActionRating(player.GetAction<UpgradeAction>(),
                                         player.UpgradeListGenerator.RateRallyPoint(player, faction.UpgradeCount*2)),
                                           
                        // Upgrade + Move
                        new ActionRating(player.GetAction<UpgradeAction>(), player.UpgradeListGenerator.RateForMove(player, player.MoveListGenerator)),

                        // Build + Move
                        new ActionRating(player.GetAction<BuildAction>(),
                                         player.BuildListGenerator.RateStagingPoint(player, BuildListGenerator.RateAttackEfficency)),
                    };
            return twoActionRatings.ChooseBest(player.ActionRatingMinimum, player.Log);
        }

        public static void SetTarget(IAiPlayer player, Sector target)
        {
            player.ThreatPoint = target;
            player.RallyPoint = target;
            var stagingPoint = player.Faction.GetClosestSectorTo(target);
            if (stagingPoint == null)
                throw new InvalidOperationException(string.Format("No path found from any of {0}'s sectors to {1}", player.Faction, target));
            player.StagingPoint = stagingPoint;
        }

    }
}