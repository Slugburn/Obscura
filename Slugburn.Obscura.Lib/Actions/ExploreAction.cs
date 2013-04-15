using System;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Actions
{
    public class ExploreAction : IAction
    {
        private readonly ILog _log;

        public ExploreAction(ILog log)
        {
            _log = log;
        }

        public override string ToString()
        {
            return "Explore";
        }

        public void Do(PlayerFaction faction)
        {
            var locations = faction.GetValidExplorationLocations();
            var location = faction.Player.ChooseSectorLocation(locations);
            if (!location.AdjacentWormholesFor(faction).Any())
            {
                new MapVisualizer(_log).Display(faction.Game.Map);
                throw new ArgumentException(string.Format("{0} is not a valid location to place a sector because there are no free adjacent wormholes", location));
            }
            var sector = faction.Game.GetSectorFor(location);
            if (sector.HasDiscovery)
                sector.DiscoveryTile = faction.Game.DiscoveryTiles.Draw();
            faction.Game.Ancients.CreateShipsFor(sector);
            location.Sector = sector;
            _log.Log("\t{0} found", sector);
            RotateToMatchWormholes(faction, sector);

            if (sector.Ancients == 0 && faction.Player.ChooseToClaimSector(sector))
            {
                faction.ClaimSector(sector);
            }
        }

        private void RotateToMatchWormholes(PlayerFaction faction, Sector sector)
        {
            var validFacings = sector.Location.AdjacentWormholesFor(faction).ToArray();
            if (validFacings.Length==0)
                throw new ArgumentException("No valid facings found");
            faction.Player.RotateSectorWormholes(sector, validFacings);
        }

        public bool IsValid(PlayerFaction faction)
        {
            return !faction.Passed && faction.Influence> 0 && faction.GetValidExplorationLocations().Any();
        }
    }
}