using NSubstitute;
using NUnit.Framework;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Test.Actions.Exploration
{
    [TestFixture]
    public class ExploreActionTest
    {
        [TestCase(true)]
        [TestCase(false)]
        public void WhenUndefendedSectorIsFound(bool playerClaimsSector)
        {
            // Arrange
            var action = new ExploreAction();
            var game = Substitute.For<Game>();
            var player = Substitute.For<Player>();
            player.Game = game;
            var mapLocation = Substitute.For<MapLocation>();
            var mapLocations = new[] {mapLocation};
            player.GetValidExplorationLocations().Returns(mapLocations);
            player.Controller.ChooseSectorLocation(mapLocations).Returns(mapLocation);
            var sector = new Sector {Location = mapLocation, Wormholes = Facing.All};
            game.GetSectorFor(mapLocation).Returns(sector);
            mapLocation.AdjacentWormholesFor(player).Returns(Facing.All);
            player.Controller.ChooseToClaimSector(sector).Returns(playerClaimsSector);

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
