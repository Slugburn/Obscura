using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Actions
{
    public class ExploreAction : IAction
    {
        public void Do(Player player)
        {
            var locations = GetValidLocations(player);
            var location = ChooseSectorLocation(locations);
            var sector = player.Game.GetSectorFor(location);
            if (sector.HasDiscovery)
                sector.DiscoveryTile = player.Game.DiscoveryTiles.Draw();
            for (var i = 0; i < sector.Ancients; i++ )
            {
                var ship = new AncientShip();
                ship.SetSector(sector);
            }
            location.Sector = sector;
            RotateToMatchWormholes(player, sector);

            if (sector.Ancients == 0 && player.Controller.ChooseToClaimSector(sector))
            {
                player.ClaimSector(sector);
            }
        }

        private void RotateToMatchWormholes(Player player, Sector sector)
        {
            var validFacings = sector.Location.AdjacentWormholesFor(player).ToArray();
            while (!sector.Wormholes.Intersect(validFacings).Any())
                sector.RotateClockwise();
        }

        private MapLocation ChooseSectorLocation(IEnumerable<MapLocation> locations)
        {
            return locations.PickRandom();
        }

        public bool IsValid(Player player)
        {
            return GetValidLocations(player).Any();
        }

        private static IEnumerable<MapLocation> GetValidLocations(Player player)
        {
            var sourceSectors = player.Sectors.Concat(player.Ships.Where(ship=>!ship.IsPinned).Select(ship => ship.Sector)).Distinct();
            return sourceSectors.SelectMany(x => x.Location.AdjacentExplorable()).Distinct();
        }
    }
}