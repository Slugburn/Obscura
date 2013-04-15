using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ShouldPassDecision : IActionDecision
    {
        private readonly UnderAttackDecision _underAttackDecision;

        public ShouldPassDecision(UnderAttackDecision underAttackDecision)
        {
            _underAttackDecision = underAttackDecision;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            
            if (faction.SpendingInfluenceWillBankrupt())
            {
                while (faction.Science > 10)
                    faction.Trade(ProductionType.Science, ProductionType.Money);
                while (faction.Material > 10)
                    faction.Trade(ProductionType.Material, ProductionType.Money);
            }

            return faction.SpendingInfluenceWillBankrupt()
                       ? new ActionDecisionResult(player.GetAction<PassAction>())
                       : new ActionDecisionResult(_underAttackDecision);
        }
    }
}
