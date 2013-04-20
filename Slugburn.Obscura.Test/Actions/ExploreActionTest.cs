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
            var faction = Substitute.For<Faction>();
            faction.Game = game;
            var mapLocation = Substitute.For<MapLocation>();
            var mapLocations = new[] {mapLocation};
            faction.GetValidExplorationLocations().Returns(mapLocations);
            faction.Player.ChooseSectorLocation(mapLocations).Returns(mapLocation);
            var sector = new Sector {Location = mapLocation, Wormholes = Facing.All};
            game.GetSectorFor(mapLocation).Returns(sector);
            mapLocation.AdjacentWormholesFor(faction).Returns(Facing.All);
            faction.Player.ChooseToClaimSector(sector).Returns(playerClaimsSector);

            // Act
            action.Do(faction);

            // Assert
            if (playerClaimsSector)
                faction.Received().ClaimSector(sector);
            else
                faction.DidNotReceive().ClaimSector(sector);
        }
    }
}
