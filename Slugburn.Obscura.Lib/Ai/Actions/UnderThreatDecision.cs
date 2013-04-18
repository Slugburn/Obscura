using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class UnderThreatDecision : IActionDecision
    {
        private readonly AttackDecision _attackDecision;
        private readonly AssaultRallyPointDecision _assaultRallyPointDecision;
        private readonly ILog _log;

        public UnderThreatDecision(AttackDecision safeDecision, AssaultRallyPointDecision assaultRallyPointDecision, ILog log)
        {
            _attackDecision = safeDecision;
            _assaultRallyPointDecision = assaultRallyPointDecision;
            _log = log;
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
                return new ActionDecisionResult(_attackDecision);

            var threatPoint = mostNeedsDefending.AdjacentSectors().OrderByDescending(x => x.GetEnemyShips(faction).GetTotalRating()).First();

            _log.Log("{0} feels threatened by {1} in {2}", faction, threatPoint.Owner, mostNeedsDefending);

            player.ThreatPoint = threatPoint;
            player.RallyPoint = mostNeedsDefending;
            player.StagingPoint = mostNeedsDefending;

            return new ActionDecisionResult(_assaultRallyPointDecision);
        }
    }
}