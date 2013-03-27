using NSubstitute;
using NUnit.Framework;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Test.Builders
{
    [TestFixture]
    public class StarbaseBuilderTest
    {
        private StarbaseBuilder _builder;
        private Faction _faction;
        private Sector _homeSector;

        [SetUp]
        public void BeforeEach()
        {
            _builder = new StarbaseBuilder();
            _faction = new Faction();
            _faction.Materials = 100;
            _faction.Starbase = new ShipBlueprint {Cost = 3};
            _homeSector = new Sector();
            _faction.ClaimSector(_homeSector);
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
            Assert.That(ship.Blueprint, Is.SameAs(_faction.Starbase));
            Assert.That(_faction.Materials, Is.EqualTo(startingMaterials - _faction.Starbase.Cost));
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
            _faction.Materials = 2;
            
            // Act
            var result = _builder.IsBuildAvailable(_faction);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_NoPlacementLocation()
        {
            // Arrange
            var ship = (PlayerShip)_builder.Create(_faction);
            _homeSector.AddShip(ship);

            // Act
            var result = _builder.IsBuildAvailable(_faction);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}