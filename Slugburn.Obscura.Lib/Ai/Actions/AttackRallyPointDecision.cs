using System;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class AttackRallyPointDecision : IActionDecision
    {
        private readonly MoveListGenerator _moveListGenerator;
        private readonly ImproveFleetDecision _improveFleetDecision;

        public AttackRallyPointDecision(MoveListGenerator moveListGenerator, ImproveFleetDecision improveFleetDecision)
        {
            _moveListGenerator = moveListGenerator;
            _improveFleetDecision = improveFleetDecision;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;

            var enemyRating = player.RallyPoint.GetEnemyShips(faction).GetTotalRating();

            // avoid overkill
            var ourRating = player.RallyPoint.GetFriendlyShips(faction).GetTotalRating();
            if (ourRating / enemyRating > 2.0m)
                return new ActionDecisionResult(new ExploreDecision());
                
            // does our entire fleet have enough power to take down enemy?
            var fleetRating = faction.Ships.GetTotalRating();
            if (fleetRating/enemyRating < 1.5m)
                return new ActionDecisionResult(_improveFleetDecision);

            // can we move enough ships to win on this round?
            // if not, redirect ships to a nearby rally point
            player.MoveList = _moveListGenerator.Generate(player).ToList();
            if (!player.MoveList.Any())
                return new ActionDecisionResult(new ExploreDecision());

            var shipsAtDestinations = player.MoveList.Take(faction.GetActionsBeforeBankruptcy()*faction.MoveCount)
                .GroupBy(x => x.Ship)
                .Select(g => new {Ship = g.Key, Final = g.Last().Destination})
                .Where(x => x.Final == player.RallyPoint)
                .Select(x => x.Ship)
                .Concat(player.RallyPoint.GetFriendlyShips(faction));
            var afterMoveRating = shipsAtDestinations.GetTotalRating();
            if (afterMoveRating/enemyRating < 1.5m)
            {
                player.RallyPoint = faction.GetClosestSectorTo(player.RallyPoint);
                player.MoveList = _moveListGenerator.Generate(player);
            }

            return new ActionDecisionResult(player.GetAction<MoveAction>());
        }
    }
}