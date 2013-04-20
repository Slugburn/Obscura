using System;
using NSubstitute;
using NUnit.Framework;
using Ninject;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Messages;
using Slugburn.Obscura.Lib.Messaging;
using Slugburn.Obscura.Lib.Players;

namespace Slugburn.Obscura.Test.Factions.Behavior
{
    [TestFixture]
    public class ClaimSectorTest
    {
        [Test]
        public void DiscoveredPartUpdatesPartList()
        {
            // Arrange
            var kernel = new StandardKernel();
            kernel.Load(new TestModule());
            var pipe = new MessagePipe();
            kernel.Rebind<IMessagePipe>().ToConstant(pipe);
            var faction = kernel.Get<Faction>();

            var player = Substitute.For<IPlayer>();
            player.ChooseToUseDiscovery(Arg.Any<Discovery>()).Returns(true);
            var discovery = Discovery.AxiomComputer;
            var sector = new Sector {DiscoveryTile = discovery};

            var messageReceived = false;
            Action<PartDiscovered> assertions = msg =>
                {
                    messageReceived = true;
                    Assert.That(msg.Part.Name, Is.EqualTo(discovery.Name));
                };

            // Act
            using (pipe.Subscribe(assertions))
                faction.ClaimSector(sector);

            // Assert
            Assert.That(messageReceived, Is.True);
        }

    }
}
