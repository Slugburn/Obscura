using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Generators;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class UnderAttack : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            var faction = state.Faction;
            var player = state.Player;

            var sectors = faction.Sectors;
            var sectorsUnderAttack = sectors.Where(s => s.GetEnemyShips(faction).Any());
            var mostNeedsDefending = (from sector in sectorsUnderAttack
                                      let ratio = faction.CombatSuccessRatio(sector)
                                      where ratio < 2
                                      orderby ratio
                                      select sector).FirstOrDefault();
            if (mostNeedsDefending == null)
                return null;

            player.ThreatPoint = mostNeedsDefending;
            player.RallyPoint = mostNeedsDefending;
            player.StagingPoint = mostNeedsDefending;

            while (faction.Money >= faction.TradeRatio)
                faction.Trade(ProductionType.Money, ProductionType.Material);

            var actionRatings = new List<ActionRating>
                                      {
                                          new ActionRating(player.GetAction<MoveAction>(), player.MoveListGenerator.Rate(player)),
                                          new ActionRating(player.GetAction<UpgradeAction>(), player.UpgradeListGenerator.RateRallyPoint(player)),
                                          new ActionRating(player.GetAction<BuildAction>(), player.BuildListGenerator.RateStagingPoint(player, BuildListGenerator.RateCombatEfficiency))
                                      };

            return actionRatings.ChooseBest(player.ActionRatingMinimum, player.Log);
        }
    }
}
