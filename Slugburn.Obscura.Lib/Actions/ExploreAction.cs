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

        public string Name { get { return "Explore"; } }

        public void Do(Faction faction)
        {
            var locations = faction.GetValidExplorationLocations();
            var location = faction.Player.ChooseSectorLocation(locations);
            var sector = faction.Game.GetSectorFor(location);
            if (sector.HasDiscovery)
                sector.DiscoveryTile = faction.Game.DiscoveryTiles.Draw();
            CreateAncientShips(sector);
            location.Sector = sector;
            _log.Log("{0}'s exploration finds {1}", faction.Name, sector);
            RotateToMatchWormholes(faction, sector);

            if (sector.Ancients == 0 && faction.Player.ChooseToClaimSector(sector))
            {
                faction.ClaimSector(sector);
            }
        }

        private static void CreateAncientShips(Sector sector)
        {
            for (var i = 0; i < sector.Ancients; i++)
            {
                var ship = new AncientShip();
                sector.AddShip(ship);
            }
        }

        private void RotateToMatchWormholes(Faction faction, Sector sector)
        {
            var validFacings = sector.Location.AdjacentWormholesFor(faction).ToArray();
            if (validFacings.Length==0)
                throw new ArgumentException("No valid facings found");
            faction.Player.RotateSectorWormholes(sector, validFacings);
        }

        public bool IsValid(Faction faction)
        {
            return !faction.Passed && faction.Influence> 0 && faction.GetValidExplorationLocations().Any();
        }
    }
}