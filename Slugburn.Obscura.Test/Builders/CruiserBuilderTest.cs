using NSubstitute;
using NUnit.Framework;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Test.Builders
{
    [TestFixture]
    public class CruiserBuilderTest
    {
        private CruiserBuilder _builder;
        private Faction _faction;

        [SetUp]
        public void BeforeEach()
        {
            _builder = new CruiserBuilder();
            _faction = Substitute.For<Faction>();
            _faction.Material = 100;
            _faction.Cruiser = new ShipBlueprint {Cost = 5};
        }

        [Test]
        public void Create()
        {
            // Arrange
            var startingMaterials = _faction.Material;

            // Act
            var ship = (PlayerShip)_builder.Create(_faction);

            // Assert
            Assert.That(ship.Owner, Is.SameAs(_faction));
            Assert.That(ship.Blueprint, Is.SameAs(_faction.Cruiser));
            Assert.That(_faction.Material, Is.EqualTo(startingMaterials - _faction.Cruiser.Cost));
        }

        [TestCase(3, true)]
        [TestCase(4, false)]
        public void IsValid_ShipCount(int existing, bool isValid)
        {
            // Arrange
            for (var i = 0; i < existing; i++)
                _builder.Create(_faction);

            // Act
            var result = _builder.IsBuildAvailable(_faction);

            // Assert
            Assert.That(result, Is.EqualTo(isValid));
        }

        [Test]
        public void IsValid_NotEnoughMaterials()
        {
            // Arrange
            _faction.Material = 4;
            
            // Act
            var result = _builder.IsBuildAvailable(_faction);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}