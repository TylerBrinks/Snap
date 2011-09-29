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
using Castle.MicroKernel;
using Microsoft.Practices.ServiceLocation;

namespace Snap.CastleWindsor
{
    /// <summary>
    /// This is common service locator adapter for Castle.Windsor IoC container
    /// </summary>
    /// <remarks>
    /// Cannot use existing adapter from CommonServiceLocator.WindsorAdapter library. 
    /// It expects IWindsorContainer to be passed in. But only IKernel is available in existing SNAP architecture.
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