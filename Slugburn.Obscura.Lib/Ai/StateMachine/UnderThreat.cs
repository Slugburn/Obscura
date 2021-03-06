﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Generators;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class UnderThreat : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            {
                var faction = state.Faction;
                var player = state.Player;

                var mostNeedsDefending = MostNeedsDefending(faction);
                if (mostNeedsDefending == null)
                    return null;

                var threatPoint = mostNeedsDefending.AdjacentSectors()
                    .Where(x => SectorThreatRating(x, faction) > 0)
                    .OrderByDescending(x => SectorThreatRating(x, faction))
                    .First();

                faction.Log("{0} feels threatened by {1} in {2}", faction, threatPoint.Owner, mostNeedsDefending);

                player.ThreatPoint = threatPoint;
                player.RallyPoint = mostNeedsDefending;
                player.StagingPoint = mostNeedsDefending;

                var actionRatings = new List<ActionRating>
                                      {
                                        // undervalue move to encourage building and upgrading
                                          new ActionRating(player.GetAction<MoveAction>(), player.MoveListGenerator.Rate(player)/2),
                                          new ActionRating(player.GetAction<UpgradeAction>(), player.UpgradeListGenerator.RateRallyPoint(player)),
                                          new ActionRating(player.GetAction<BuildAction>(), player.BuildListGenerator.RateStagingPoint(player, BuildListGenerator.RateCombatEfficiency))
                                      };

                return actionRatings.ChooseBest(player.ActionRatingMinimum, player.Log);
            }
        }

        private static decimal SectorThreatRating(Sector x, Faction faction)
        {
            return x.GetEnemyShips(faction).Where(s=> s.Owner is Faction && s.Move > 0).GetTotalRating();
        }

        public static Sector MostNeedsDefending(Faction faction)
        {
            var threatenedSectors = from sector in faction.Sectors
                                    let adjacent = (
                                                       from adj in sector.AdjacentSectors()
                                                       where SectorThreatRating(adj, faction) > 0
                                                       select adj
                                                   )
                                    where adjacent.Any()
                                    select new {sector, adjacent};

            var mostNeedsDefending = (from location in threatenedSectors
                                      let ratio = faction.CombatSuccessRatio(location.sector, location.adjacent)
                                      where ratio < 0.5m
                                      orderby ratio
                                      select location.sector).FirstOrDefault();
            return mostNeedsDefending;
        }
    }
}
