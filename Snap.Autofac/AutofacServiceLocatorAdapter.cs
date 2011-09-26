using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.Practices.ServiceLocation;

namespace Snap.Autofac
{
    /// <summary>
    /// This is common service locator adapter for Autofac IoC container
    /// </summary>
    /// <remarks>
    /// AutofacContrib.CommonServiceLocator library, which is redistributed as a part of AutofacContrib, cannot be used.
    /// Unfortunately, Autofac.CommonServiceLocator which is compatible with Autofac 2.4.5.724  is build for NET 4.0 runtime
    /// This makes impossible to use it under NET 3.5 runtime.
    /// See Issue 288: 	No Autofac-Contrib 2.4.3-700 release for .net 3.5
    /// http://code.google.com/p/autofac/issues/detail?id=288#c2
    ///  
    /// The most recent Autofac.CommonServiceLocator, which is built for NET3.5 runtime, is of version 2.3.2. 
    /// See http://code.google.com/p/autofac/downloads/detail?name=AutofacContrib-2.3.2.632-NET35.zip&can=1&q=
    /// But it has a reference to Autofac 2.3.2.632. Both assemblies have strong name. 
    /// Thus, it requires assembly version redirection to use 2.4.5.724.
    /// 
    /// All described above is too complicated. It is much simple to write custom autofac service locator adapter (few lines of code).
    /// Once Autofac.CommonServiceLocator is redistributed for NET35 runtime, switch using it instead of this class.
    /// </remarks>
    public class AutofacServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly IComponentContext _container;

        public AutofacServiceLocatorAdapter(IComponentContext container)
        {
            _container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key != null 
                ? _container.ResolveNamed(key, serviceType) 
                : _container.Resolve(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);

            return ((IEnumerable)_container.Resolve(enumerableType)).Cast<object>();
        }
    }
}