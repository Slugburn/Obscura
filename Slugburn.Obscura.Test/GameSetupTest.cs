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
            kernel.Bind<ILog>().To<ConsoleLog>().InSingletonScope();
            var game = kernel.Get<Game>();
            var factions = new[] { kernel.Get<Faction>(), kernel.Get<Faction>() };

            // Act
            game.Setup(factions);
            game.Start();
        }

    }
}
