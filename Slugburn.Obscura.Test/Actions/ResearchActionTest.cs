using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Test.Actions
{
    [TestFixture]
    public class ResearchActionTest
    {
        private ResearchAction _action;
        private Faction _faction;
        private Tech _tech;
        private Tech[] _availableTech;
        private Game _game;

        [SetUp]
        public void BeforeEach()
        {
            _action = new ResearchAction(new ConsoleLog());
            _tech = new Tech("Test", 4, 3, TechCategory.Grid);
            _game = Substitute.For<Game>();
            _game.AvailableTechTiles = new List<Tech> {_tech};
            _faction = Substitute.For<Faction>();
            _faction.Game = _game;
            _availableTech = new[] { _tech };
            _faction.AvailableResearchTech().Returns(_availableTech);
            _faction.Player.ChooseResearch(_availableTech).Returns(_tech);
            _faction.CostFor(_tech).Returns(4);
        }

        [Test]
        public void PlayerChoosesFromAvailableTech()
        {
            // Act
            _action.Do(_faction);

            // Assert
            _faction.Player.Received().ChooseResearch(_availableTech);
        }

        [Test]
        public void ChosenTechIsAddedToPlayerTech()
        {
            // Act
            _action.Do(_faction);

            // Assert
            Assert.That(_faction.Technologies, Has.Member(_tech));
        }

        [Test]
        public void PlayerScienceReducedByTechCost()
        {
            // Arrange
            const int startingScience = 10;
            _faction.Science = startingScience;

            // Act
            _action.Do(_faction);

            // Assert
            Assert.That(_faction.Science, Is.EqualTo(startingScience - _faction.CostFor(_tech)));
        }

        [Test]
        public void TechIsRemovedFromGamesAvailableTech()
        {
            // Act
            _action.Do(_faction);

            // Assert
            Assert.That(_game.AvailableTechTiles, Has.No.Member(_tech));
        }

    }
}
