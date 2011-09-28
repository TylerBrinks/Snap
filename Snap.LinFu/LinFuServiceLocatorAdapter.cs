using System;
using System.Collections.Generic;
using System.Linq;
using LinFu.IoC;
using LinFu.IoC.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace Snap.LinFu
{
    /// <summary>
    /// This is common service locator adapter for Castle.Windsor IoC container
    /// </summary>
    /// <remarks>
    /// Common service locator adapter for LinFu is redistributed as source code.
    /// See: http://code.google.com/p/linfu/downloads/list
    /// </remarks>
    public sealed class LinFuServiceLocatorAdapter : ServiceLocatorImplBase
    {
        readonly IServiceContainer _container;

        public LinFuServiceLocatorAdapter(IServiceContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key != null 
                ? _container.GetService(key, serviceType) 
                : _container.GetService(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _container.AvailableServices
                .Where(info => serviceType == info.ServiceType && (info.ArgumentTypes == null || info.ArgumentTypes.Count() == 0))
                .Select(service => _container.GetService(service));
        }
    }
}
