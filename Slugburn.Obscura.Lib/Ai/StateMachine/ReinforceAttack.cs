using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class ReinforceAttack : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            var faction = state.Faction;
            var player = state.Player;

            var sectors = faction.Ships.Select(x=>x.Sector).Distinct();
            var combatSectors = sectors.Where(s => s.GetEnemyShips(faction).Any());
            var mostNeedsReinforcement = (from sector in combatSectors
                                      let ratio = faction.CombatSuccessRatio(sector)
                                      where ratio < 2
                                      orderby ratio
                                      select sector).FirstOrDefault();
            if (mostNeedsReinforcement == null)
                return null;

            while (faction.Money >= faction.TradeRatio)
                faction.Trade(ProductionType.Money, ProductionType.Material);

            Attack.SetTarget(player, mostNeedsReinforcement);

            return Attack.ChooseBestAction(player);
        }
    }
}
