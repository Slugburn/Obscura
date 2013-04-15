using NSubstitute;
using NUnit.Framework;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Test.Builders
{
    [TestFixture]
    public class InterceptorBuilderTest
    {
        private InterceptorBuilder _builder;
        private PlayerFaction _faction;

        [SetUp]
        public void BeforeEach()
        {
            _builder = new InterceptorBuilder();
            _faction = Substitute.For<PlayerFaction>();
            _faction.Material = 100;
            _faction.Interceptor = new ShipBlueprint {Cost = 3};
        }

        [Test]
        public void Create()
        {
            // Arrange
            var startingMaterials = _faction.Material;

            // Act
            var ship = (PlayerShip)_builder.Create(_faction);

            // Assert
            Assert.That(ship.Faction, Is.SameAs(_faction));
            Assert.That(ship.Blueprint, Is.SameAs(_faction.Interceptor));
            Assert.That(_faction.Material, Is.EqualTo(startingMaterials - _faction.Interceptor.Cost));
        }

        [TestCase(7, true)]
        [TestCase(8, false)]
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
            _faction.Material = 2;
            
            // Act
            var result = _builder.IsBuildAvailable(_faction);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
