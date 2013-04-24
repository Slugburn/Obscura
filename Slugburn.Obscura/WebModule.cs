using System.Collections.Generic;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Views.Main;

namespace Slugburn.Obscura
{
    public class WebModule : NinjectModule
    {

        public override void Load()
        {
            Bind<ILog>().To<CaptureLog>().InSingletonScope();
            Bind<IGameView>().To<GameView>().InSingletonScope();
            Kernel.Bind(x => x.FromAssemblyContaining<Game>().SelectAllClasses().BindAllInterfaces());
        }
    }

    public class CaptureLog :ILog
    {
        public CaptureLog()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; set; }

        public void Log(string messageFormat, params object[] args)
        {
//            var message = string.Format(messageFormat, args);
//            Messages.Add(message);
        }
    }
}