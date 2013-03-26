using NSubstitute;
using NUnit.Framework;
using Slugburn.Obscura.Lib.Builds;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Test.Builders
{
    [TestFixture]
    public class DreadnoughtBuilderTest
    {
        private DreadnoughtBuilder _builder;
        private Faction _faction;

        [SetUp]
        public void BeforeEach()
        {
            _builder = new DreadnoughtBuilder();
            _faction = Substitute.For<Faction>();
            _faction.Materials = 100;
            _faction.Dreadnought = new ShipBlueprint {Cost = 8};
        }

        [Test]
        public void Create()
        {
            // Arrange
            var startingMaterials = _faction.Materials;

            // Act
            var ship = (PlayerShip)_builder.Create(_faction);

            // Assert
            Assert.That(ship.Faction, Is.SameAs(_faction));
            Assert.That(ship.Blueprint, Is.SameAs(_faction.Dreadnought));
            Assert.That(_faction.Materials, Is.EqualTo(startingMaterials - _faction.Dreadnought.Cost));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public void IsValid_ShipCount(int existing, bool isValid)
        {
            // Arrange
            for (var i = 0; i < existing; i++)
                _builder.Create(_faction);

            // Act
            var result = _builder.IsValid(_faction);

            // Assert
            Assert.That(result, Is.EqualTo(isValid));
        }

        [Test]
        public void IsValid_NotEnoughMaterials()
        {
            // Arrange
            _faction.Materials = 7;
            
            // Act
            var result = _builder.IsValid(_faction);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}