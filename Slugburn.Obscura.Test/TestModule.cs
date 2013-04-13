using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Ai.Actions;

namespace Slugburn.Obscura.Test
{
    public class TestModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().To<ConsoleLog>().InSingletonScope();
            Bind<IActionDecision>().To<ShouldPassDecision>().InSingletonScope().Named("root");
            Kernel.Bind(x => x.FromAssemblyContaining<Game>().SelectAllClasses().BindAllInterfaces());
        }
    }
}