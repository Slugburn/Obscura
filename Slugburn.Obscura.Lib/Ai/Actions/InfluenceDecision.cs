using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class InfluenceDecision : IActionDecision
    {
        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            player.Faction.Colonize();
            player.InfluenceList = player.Faction.GetInfluencePlacementLocations()
                                         .Select(x => new InfluenceLocation {Location = x})
                                         .ToList();
            return new ActionDecisionResult(player.GetAction<InfluenceAction>());
        }
    }
}
