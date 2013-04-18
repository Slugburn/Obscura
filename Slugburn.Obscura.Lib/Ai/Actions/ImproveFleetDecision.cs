using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Generators;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ImproveFleetDecision : IActionDecision
    {
        private readonly BuildListGenerator _buildListGenerator;
        private readonly UpgradeListGenerator _upgradeListGenerator;
        private readonly ResearchListGenerator _researchListGenerator;
        private readonly ILog _log;

        public ImproveFleetDecision(
            BuildListGenerator buildListGenerator, 
            UpgradeListGenerator upgradeListGenerator, 
            ResearchListGenerator researchListGenerator,
            ILog log)
        {
            _buildListGenerator = buildListGenerator;
            _upgradeListGenerator = upgradeListGenerator;
            _researchListGenerator = researchListGenerator;
            _log = log;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            // possible actions are building ships, upgrading ships, researching new technology
            var actionRatings = new[]
                                    {
                                        new ActionRating(player.GetAction<BuildAction>(),
                                                         _buildListGenerator.Rate(player, BuildListGenerator.RateAttackEfficency)),
                                        new ActionRating(player.GetAction<UpgradeAction>(), _upgradeListGenerator.RateFleet(player)),
                                        new ActionRating(player.GetAction<ResearchAction>(), _researchListGenerator.RateFleet(player)),
                                    };
            return actionRatings.ChooseBest(_log);
        }
    }
}