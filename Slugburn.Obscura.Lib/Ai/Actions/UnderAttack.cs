using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class UnderAttack : IActionDecision
    {
        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var attackLocations = faction.Sectors.Where(s => s.GetEnemyShips(faction).Any());
            var mostNeedsDefending = (from location in attackLocations
                                      let myValue = location.GetFriendlyShips(faction).Sum(x => x.Profile.Rating)
                                      let enemyValue = location.GetEnemyShips(faction).Sum(x => x.Profile.Rating)
                                      let ratio = myValue/enemyValue
                                      where ratio < 2
                                      orderby ratio
                                      select location).FirstOrDefault();
            if (mostNeedsDefending == null)
                return new DecisionResult<IAction>(new UnderThreat());
            player.RallyPoint = mostNeedsDefending;
            return new DecisionResult<IAction>(new DefendRallyPoint());
        }
    }
}
