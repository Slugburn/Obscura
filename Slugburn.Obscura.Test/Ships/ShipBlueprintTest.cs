using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Test.Ships
{
    [TestFixture]
    public class ShipBlueprintTest
    {
        [Test]
        public void CanUsePartToUpgrade()
        {
            /* 
Current: Ion Cannon, Nuclear Drive, Nuclear Source
Ideal  : Axiom Computer, Fusion Drive, Ion Cannon, Nuclear Source
Upgrade: Axiom Computer
Replace: Nuclear Drive
             */

            // Arrange 
            var blueprint = new ShipBlueprint
                                {
                                    PartSpaces = 4,
                                    Parts = new List<ShipPart>()
                                                {
                                                    PartFactory.IonCannon(),
                                                    PartFactory.NuclearDrive(),
                                                    PartFactory.NuclearSource()
                                                }
                                };
            var axiomComputer = new AncientShipPart {Name = "Axiom Computer", Accuracy = 3};

            // Act
            var result = blueprint.CanUsePartToUpgrade(axiomComputer);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
