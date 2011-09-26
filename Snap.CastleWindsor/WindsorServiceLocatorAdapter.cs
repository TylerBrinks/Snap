using System;
using System.Collections.Generic;
using Castle.MicroKernel;
using Microsoft.Practices.ServiceLocation;

namespace Snap.CastleWindsor
{
    /// <summary>
    /// This is common service locator adapter for Castle.Windsor IoC container
    /// </summary>
    /// <remarks>
    /// Cannot use existing adapter from CommonServiceLocator.WindsorAdapter library. 
    /// It expects IWindsorContainer to be created with. But only IKernel is available in existing SNAP architecture.
    /// 
    /// Thus, decide to build own simple adapter, which accepts IKernel and resolve types against it.
    /// </remarks>
    public class WindsorServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly IKernel _kernel;

        public WindsorServiceLocatorAdapter(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key != null
                ? _kernel.Resolve(key, serviceType)
                : _kernel.Resolve(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return (IEnumerable<object>)_kernel.ResolveAll(serviceType);
        }
    }
 }