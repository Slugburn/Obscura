using NUnit.Framework;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Test.Factions.Behavior
{
    [TestFixture]
    public class CostForTechTest
    {
        private Tech _tech;
        private Faction _faction;

        [SetUp]
        public void BeforeEach()
        {
            _tech = new Tech("Test", 6, 4, TechCategory.Grid);
            _faction = new Faction(new ConsoleLog(), null);
        }

        [Test]
        public void CostIsEqualToTechCost()
        {
            // Act
            var cost = _faction.CostFor(_tech);

            // Assert
            Assert.That(cost, Is.EqualTo(_tech.Cost));
        }

        [TestCase(0, 0)]
        [TestCase(1, -1)]
        [TestCase(2, -2)]
        [TestCase(3, -3)]
        [TestCase(4, -4)]
        [TestCase(5, -6)]
        [TestCase(6, -8)]
        public void CostIsReducedByDiscount(int existingInCategory, int expectedDiscount)
        {
            // Arrange
            for (var i = 0; i < existingInCategory; i++)
                _faction.Technologies.Add(new Tech("Test" + i, i, i, TechCategory.Grid));
            _tech.Cost = 16;

            // Act
            var cost = _faction.CostFor(_tech);

            // Assert
            Assert.That(cost, Is.EqualTo(_tech.Cost + expectedDiscount));
        }

        [Test]
        public void CostIsNotReducedBelowMinimum()
        {
            // Arrange
            for (var i = 0; i < 4; i++)
                _faction.Technologies.Add(new Tech("Test" + i, i, i, TechCategory.Grid));

            // Act
            var cost = _faction.CostFor(_tech);

            // Assert
            Assert.That(cost, Is.EqualTo(_tech.MinCost));
        }

        [Test]
        public void TechnologiesInAnotherCategoryDoNotProvideDiscount()
        {
            // Arrange
            _faction.Technologies.Add(new Tech("Other tech", 8,5,TechCategory.Military));

            // Act
            var cost = _faction.CostFor(_tech);

            // Assert
            Assert.That(cost, Is.EqualTo(_tech.Cost));
        }
    }
}
