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
        private Faction _faction;

        [SetUp]
        public void BeforeEach()
        {
            _builder = new InterceptorBuilder();
            _faction = Substitute.For<Faction>();
            _faction.Materials = 100;
            _faction.Interceptor = new ShipBlueprint {Cost = 3};
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
            Assert.That(ship.Blueprint, Is.SameAs(_faction.Interceptor));
            Assert.That(_faction.Materials, Is.EqualTo(startingMaterials - _faction.Interceptor.Cost));
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
            _faction.Materials = 2;
            
            // Act
            var result = _builder.IsBuildAvailable(_faction);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
