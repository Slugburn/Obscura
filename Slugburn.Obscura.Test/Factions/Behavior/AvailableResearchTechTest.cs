using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Test.Factions.Behavior
{
    [TestFixture]
    public class AvailableResearchTechTest
    {
        private Game _game;
        private PlayerFaction _faction;
        private Tech _tech;

        [SetUp]
        public void BeforeEach()
        {
            _game = Substitute.For<Game>();
            _tech = new Tech("Test", 4, 3, TechCategory.Grid);
            _game.AvailableTechTiles = new List<Tech> { _tech };

            _faction = new PlayerFaction(new ConsoleLog(), null) { Game = _game, Science = 4 };
        }

        [Test]
        public void TechFromGamePoolAreAvailable()
        {
            // Act
            var results = _faction.AvailableResearchTech();

            // Assert
            Assert.That(results, Is.EquivalentTo(_game.AvailableTechTiles));
        }

        [Test]
        public void TechThePlayerAlreadyHasIsNotAvailable()
        {
            // Arrange
            _faction.Technologies.Add(_tech);

            // Act
            var results = _faction.AvailableResearchTech();

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void TechThatIsTooExpensiveIsNotAvailable()
        {
            // Arrange
            _faction.Science = 3;

            // Act
            var results = _faction.AvailableResearchTech();

            // Assert
            Assert.That(results, Is.Empty);
        }
    }
}
