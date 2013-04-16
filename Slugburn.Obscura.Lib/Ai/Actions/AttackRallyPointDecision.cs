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
        private readonly ILog _log;

        public AttackRallyPointDecision(MoveListGenerator moveListGenerator, ImproveFleetDecision improveFleetDecision, ILog log)
        {
            _moveListGenerator = moveListGenerator;
            _improveFleetDecision = improveFleetDecision;
            _log = log;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;

            var enemyRating = player.RallyPoint.GetEnemyShips(faction).GetTotalRating();

            // avoid overkill
            var ourRating = player.RallyPoint.GetFriendlyShips(faction).GetTotalRating();
            if (ourRating / enemyRating > 2.0m)
                return new ActionDecisionResult(new ExploreDecision());
                
            // does our entire unengaged fleet have enough power to take down enemy?
            var fleetRating = faction.Ships.Where(x =>
                {
                    var enemyShips = x.Sector.GetEnemyShips(faction);
                    return !enemyShips.Any();
                }).GetTotalRating();
            if (fleetRating/enemyRating < 2.0m)
            {
                _log.Log("{0} decides to upgrade their fleet", faction);
                return new ActionDecisionResult(_improveFleetDecision);
            }

            // can we move enough ships to win on this round?
            // if not, redirect ships to a nearby rally point
            player.MoveList = _moveListGenerator.Generate(player).ToList();
            if (!player.MoveList.Any())
                return new ActionDecisionResult(new ExploreDecision());

            var shipsAtDestinations = player.MoveList.Take(faction.GetActionsBeforeBankruptcy()*faction.MoveCount)
                .GroupBy(x => x.Ship)
                .Select(g => new {Ship = g.Key, Final = g.Last().Moves})
                .Where(x => x.Final.Last() == player.RallyPoint)
                .Select(x => x.Ship)
                .Concat(player.RallyPoint.GetFriendlyShips(faction));
            var afterMoveRating = shipsAtDestinations.GetTotalRating();
            if (afterMoveRating/enemyRating < 2.0m)
            {
                player.RallyPoint = faction.GetClosestSectorTo(player.RallyPoint);
                player.MoveList = _moveListGenerator.Generate(player);
            }

            return new ActionDecisionResult(player.GetAction<MoveAction>());
        }
    }
}