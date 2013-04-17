using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class AssaultRallyPointDecision : IActionDecision
    {
        private readonly MoveListGenerator _moveListGenerator;
        private readonly ImproveFleetDecision _improveFleetDecision;
        private readonly ILog _log;

        public AssaultRallyPointDecision(MoveListGenerator moveListGenerator, ImproveFleetDecision improveFleetDecision, ILog log)
        {
            _moveListGenerator = moveListGenerator;
            _improveFleetDecision = improveFleetDecision;
            _log = log;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;

            // does our entire unengaged fleet have enough power to take down enemy?
            if (!faction.FleetCanConquer(player.ThreatPoint))
            {
                _log.Log("{0} decides to upgrade their fleet", faction);
                return new ActionDecisionResult(_improveFleetDecision);
            }

            player.MoveList = _moveListGenerator.Generate(player).ToList();
            if (!player.MoveList.Any())
                return new ActionDecisionResult(player.GetAction<PassAction>());

            if (player.ThreatPoint == player.RallyPoint && player.ThreatPoint.Owner != faction)
            {
                // can we move enough ships to win on this round?
                // if not, redirect ships to a nearby rally point
                var shipsAtDestinations = player.MoveList.Take(faction.GetActionsBeforeBankruptcy() * faction.MoveCount)
                                                              .GroupBy(x => x.Ship)
                                                              .Select(g => new { Ship = g.Key, Final = g.Last().Moves })
                                                              .Where(x => x.Final.Last() == player.RallyPoint)
                                                              .Select(x => x.Ship)
                                                              .Concat(player.RallyPoint.GetFriendlyShips(faction));
                var afterMoveRating = shipsAtDestinations.GetTotalRating();

                var enemyRating = faction.GetEnemyRatingFor(player.ThreatPoint);
                if (afterMoveRating / enemyRating < 2.0m)
                {
                    player.RallyPoint = faction.GetClosestSectorTo(player.ThreatPoint);
                    player.MoveList = _moveListGenerator.Generate(player);
                }
            }

            return new ActionDecisionResult(player.GetAction<MoveAction>());
        }
    }
}