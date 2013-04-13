using System.Linq;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class UnderThreatDecision : IActionDecision
    {
        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var threatenedSectors = from sector in faction.Sectors
                                    let adjacent = (
                                                       from adj in sector.Location.AdjacentSectors()
                                                       where adj.GetEnemyShips(faction).Where(s=>s.Faction!=null).Any()
                                                       select adj
                                                   )
                                    where adjacent.Any()
                                    select new {sector, adjacent};

            var mostNeedsDefending = (from location in threatenedSectors
                                      let ratio = faction.CombatSuccessRatio(location.sector, location.adjacent)
                                      where ratio < 1.5
                                      orderby ratio
                                      select location.sector).FirstOrDefault();
            if (mostNeedsDefending == null)
                return new ActionDecisionResult(new SafeDecision());
            player.RallyPoint = mostNeedsDefending;
            return new ActionDecisionResult(new DefendRallyPointDecision());
        }
    }
}