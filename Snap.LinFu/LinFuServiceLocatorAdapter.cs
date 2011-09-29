/*
Snap v1.0

Copyright (c) 2010 Tyler Brinks

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
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
