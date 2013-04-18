using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Generators;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class AssaultRallyPointDecision : IActionDecision
    {
        private readonly MoveListGenerator _moveListGenerator;
        private readonly UpgradeListGenerator _upgradeListGenerator;
        private readonly BuildListGenerator _buildListGenerator;
        private readonly ResearchListGenerator _researchListGenerator;
        private readonly ImproveFleetDecision _improveFleetDecision;
        private readonly DefendOwnSectorDecision _defendOwnSectorDecision;
        private readonly ILog _log;

        public AssaultRallyPointDecision(
            MoveListGenerator moveListGenerator, 
            UpgradeListGenerator upgradeListGenerator,
            BuildListGenerator buildListGenerator,
            ResearchListGenerator researchListGenerator,
            ImproveFleetDecision improveFleetDecision, 
            DefendOwnSectorDecision defendOwnSectorDecision, 
            ILog log)
        {
            _moveListGenerator = moveListGenerator;
            _upgradeListGenerator = upgradeListGenerator;
            _buildListGenerator = buildListGenerator;
            _researchListGenerator = researchListGenerator;
            _improveFleetDecision = improveFleetDecision;
            _defendOwnSectorDecision = defendOwnSectorDecision;
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

            if (player.RallyPoint == player.StagingPoint)
                return new ActionDecisionResult(_defendOwnSectorDecision);

            if (player.GetActionsBeforeBankruptcy() < 2)
            {
                var oneActionRatings = new[]
                                        {
                                            new ActionRating(player.GetAction<MoveAction>(), _moveListGenerator.Rate(player)),
                                            new ActionRating(player.GetAction<UpgradeAction>(), _upgradeListGenerator.RateRallyPoint(player)),
                                        };
                return oneActionRatings.ChooseBest(_log);
            }

            var twoActionRatings = new[]
                                       {
                                           // Move + Move
                                           new ActionRating(player.GetAction<MoveAction>(), _moveListGenerator.Rate(player, faction.MoveCount*2)),

                                           // Upgrade + Upgrade
                                           new ActionRating(player.GetAction<UpgradeAction>(), _upgradeListGenerator.RateRallyPoint(player, faction.UpgradeCount*2)),
                                           
                                           // Upgrade + Move
                                           new ActionRating(player.GetAction<UpgradeAction>(), _upgradeListGenerator.RateForMove(player, _moveListGenerator)),

                                           // Build + Move
                                           new ActionRating(player.GetAction<BuildAction>(), _buildListGenerator.Rate(player, BuildListGenerator.RateAttackEfficency)),

                                           // Research + Upgrade
                                           new ActionRating(player.GetAction<ResearchAction>(), _researchListGenerator.RateRallyPointForUpgrade(player))
                                       };
            return twoActionRatings.ChooseBest(_log);
        }
    }
}