using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Test
{
    public class TestModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().To<ConsoleLog>().InSingletonScope();
            Bind<IGameView>().To<ConsoleGameView>().InSingletonScope();
            Kernel.Bind(x => x.FromAssemblyContaining<Game>().SelectAllClasses().BindAllInterfaces());
        }
    }
}