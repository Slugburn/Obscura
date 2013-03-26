using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Actions
{
    public class ExploreAction : IAction
    {
        public void Do(Player player)
        {
            var locations = player.GetValidExplorationLocations();
            var location = player.Controller.ChooseSectorLocation(locations);
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
            player.Controller.RotateSectorWormholes(sector, validFacings);
        }

        public bool IsValid(Player player)
        {
            return player.GetValidExplorationLocations().Any();
        }
    }
}