using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Ninject;

namespace Slugburn.Obscura.App_Start
{
    public class NinjectSignalrDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectSignalrDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetBindings(serviceType).Any() 
                       ? _kernel.GetAll(serviceType) 
                       : base.GetServices(serviceType);
        }
    }
}