using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class UnderThreatDecision : IActionDecision
    {
        private readonly SafeDecision _safeDecision;
        private readonly AssaultRallyPointDecision _assaultRallyPointDecision;

        public UnderThreatDecision(SafeDecision safeDecision, AssaultRallyPointDecision assaultRallyPointDecision)
        {
            _safeDecision = safeDecision;
            _assaultRallyPointDecision = assaultRallyPointDecision;
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
                                      where ratio < 0.5m
                                      orderby ratio
                                      select location.sector).FirstOrDefault();
            if (mostNeedsDefending == null)
                return new ActionDecisionResult(_safeDecision);

            player.ThreatPoint = mostNeedsDefending.AdjacentSectors().OrderByDescending(x => x.GetEnemyShips(faction).GetTotalRating()).First();
            player.RallyPoint = mostNeedsDefending;
            player.StagingPoint = mostNeedsDefending;

            return new ActionDecisionResult(_assaultRallyPointDecision);
        }
    }
}