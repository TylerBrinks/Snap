using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace Snap.Ninject
{
    /// <summary>
    /// This is common service locator adapter for Ninject IoC container
    /// </summary>
    /// <remarks>
    /// Cannot use existing adapter from NInjectAdapter library distributed within CommonServiceLocator.NInjectAdapter NuGet package. 
    /// Existing NinjectAdapter is build for NET4.0 runtime. 
    /// </remarks>
    public class NinjectServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly IKernel _kernel;

        public NinjectServiceLocatorAdapter(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return _kernel.Get(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}