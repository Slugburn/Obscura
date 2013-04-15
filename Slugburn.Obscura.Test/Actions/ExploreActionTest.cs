using NSubstitute;
using NUnit.Framework;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Test.Actions
{
    [TestFixture]
    public class ExploreActionTest
    {
        [TestCase(true)]
        [TestCase(false)]
        public void WhenUndefendedSectorIsFound(bool playerClaimsSector)
        {
            // Arrange
            var action = new ExploreAction(new ConsoleLog());
            var game = Substitute.For<Game>();
            var player = Substitute.For<PlayerFaction>();
            player.Game = game;
            var mapLocation = Substitute.For<MapLocation>();
            var mapLocations = new[] {mapLocation};
            player.GetValidExplorationLocations().Returns(mapLocations);
            player.Player.ChooseSectorLocation(mapLocations).Returns(mapLocation);
            var sector = new Sector {Location = mapLocation, Wormholes = Facing.All};
            game.GetSectorFor(mapLocation).Returns(sector);
            mapLocation.AdjacentWormholesFor(player).Returns(Facing.All);
            player.Player.ChooseToClaimSector(sector).Returns(playerClaimsSector);

            // Act
            action.Do(player);

            // Assert
            if (playerClaimsSector)
                player.Received().ClaimSector(sector);
            else
                player.DidNotReceive().ClaimSector(sector);
        }
    }
}
