using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ShouldPassDecision : IActionDecision
    {
        private readonly UnderAttackDecision _underAttack;

        public ShouldPassDecision(UnderAttackDecision underAttack)
        {
            _underAttack = underAttack;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            faction.Colonize();

            if (faction.Influence <= 1)
                return new ActionDecisionResult(player.GetAction<PassAction>());
            
            if (player.SpendingInfluenceWillBankrupt())
            {
                while (faction.Science >= 20)
                    faction.Trade(ProductionType.Science, ProductionType.Money);
                while (faction.Material >= 20)
                    faction.Trade(ProductionType.Material, ProductionType.Money);
            }
            
            return player.SpendingInfluenceWillBankrupt()
                       ? new ActionDecisionResult(player.GetAction<PassAction>())
                       : new ActionDecisionResult(_underAttack);
        }
    }
}
