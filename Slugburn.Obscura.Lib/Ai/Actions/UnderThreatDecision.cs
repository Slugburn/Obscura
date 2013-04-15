using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class UnderThreatDecision : IActionDecision
    {
        private readonly SafeDecision _safeDecision;
        private readonly DefendRallyPointDecision _defendRallyPointDecision;

        public UnderThreatDecision(SafeDecision safeDecision, DefendRallyPointDecision defendRallyPointDecision)
        {
            _safeDecision = safeDecision;
            _defendRallyPointDecision = defendRallyPointDecision;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var threatenedSectors = from sector in faction.Sectors
                                    let adjacent = (
                                                       from adj in sector.AdjacentSectors()
                                                       where adj.GetEnemyShips(faction).Where(s=>s.Faction is PlayerFaction).Any()
                                                       select adj
                                                   )
                                    where adjacent.Any()
                                    select new {sector, adjacent};

            var mostNeedsDefending = (from location in threatenedSectors
                                      let ratio = faction.CombatSuccessRatio(location.sector, location.adjacent)
                                      where ratio < 1.5m
                                      orderby ratio
                                      select location.sector).FirstOrDefault();
            if (mostNeedsDefending == null)
                return new ActionDecisionResult(_safeDecision);
            player.RallyPoint = mostNeedsDefending;
            return new ActionDecisionResult(_defendRallyPointDecision);
        }
    }
}