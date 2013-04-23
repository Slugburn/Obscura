using System;
using System.Collections.Generic;
using Ninject;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

namespace Slugburn.Obscura.App_Start
{
    public class NinjectMvcDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectMvcDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}