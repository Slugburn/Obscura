using System;
using System.Collections.Generic;
using System.Web;
using Ninject;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura
{
    public class WebModule : NinjectModule
    {

        public override void Load()
        {
            Bind<ILog>().To<CaptureLog>().InSingletonScope();
            Bind<IMapVisualizer>().To<CaptureMapVisualizer>().InSingletonScope();
            Kernel.Bind(x => x.FromAssemblyContaining<Game>().SelectAllClasses().BindAllInterfaces());
        }
    }

    public class CaptureMapVisualizer : IMapVisualizer
    {
        public void Display(SectorMap map)
        {
            Map= map;
        }

        public SectorMap Map { get; set; }
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
            Messages.Add(string.Format(messageFormat, args));
        }
    }
}