using NUnit.Framework;
using Ninject;
using Ninject.Extensions.Conventions;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Test
{
    [TestFixture]
    public class GameSetupTest
    {
        [Test]
        public void Start()
        {
            // Arrange
            var kernel = new StandardKernel();
            kernel.Bind(x=>x.FromAssemblyContaining<Game>().SelectAllClasses().BindAllInterfaces());
            var game = kernel.Get<Game>();
            var factions = new[] { new Faction(), new Faction() };

            // Act
            game.Setup(factions);
            game.StartTurn();
        }

    }
}
