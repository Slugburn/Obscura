using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Generators;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class DefendOwnSectorDecision : IActionDecision
    {
        private readonly MoveListGenerator _moveListGenerator;
        private readonly UpgradeListGenerator _upgradeListGenerator;
        private readonly BuildListGenerator _buildListGenerator;
        private readonly ILog _log;

        public DefendOwnSectorDecision(MoveListGenerator moveListGenerator, UpgradeListGenerator upgradeListGenerator, BuildListGenerator buildListGenerator,
                                       ILog log)
        {
            _moveListGenerator = moveListGenerator;
            _upgradeListGenerator = upgradeListGenerator;
            _buildListGenerator = buildListGenerator;
            _log = log;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var actionRatings = new List<ActionRating>
                                      {
                                          new ActionRating(player.GetAction<MoveAction>(), _moveListGenerator.Rate(player)),
                                          new ActionRating(player.GetAction<UpgradeAction>(), _upgradeListGenerator.RateRallyPoint(player)),
                                          new ActionRating(player.GetAction<BuildAction>(), _buildListGenerator.Rate(player, BuildListGenerator.RateCombatEfficiency))
                                      };

            return actionRatings.ChooseBest(_log);
        }
    }
}