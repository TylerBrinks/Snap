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
using Castle.DynamicProxy;
using StructureMap;
using StructureMap.Interceptors;

namespace Snap.StructureMap {
    /// <summary>
    /// StructureMap type interceptor
    /// </summary>
    public class StructureMapAspectInterceptor: TypeInterceptor 
    {
        public IContainer Container { get; set; }
        private readonly ProxyFactory _proxyFactory = new ProxyFactory(new ProxyGenerator());

        public StructureMapAspectInterceptor() {
            Container = ObjectFactory.Container;
        }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        internal AspectConfiguration Configuration { get; set; }

        /// <summary>
        /// Wraps interfaces in a Castle dynamic proxy
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public object Process(object target, IContext context) 
        {
            var proxy = (MasterProxy)Container.GetInstance<IMasterProxy>();
            
            if(target.IsDecorated(proxy.Configuration))
            {
                return _proxyFactory.CreateProxy(target, proxy);
            }

            var name = target.GetType().FullName;
            // Don't build up any wrapped proxy types.
            if(!(name.StartsWith("Castle.Proxies.") && name.EndsWith("Proxy"))) {
                context.BuildUp(target);
            }

            return target;
        }
        /// <summary>
        /// Matcheses types in the a namespace that implement IInterceptAspect.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool MatchesType(Type type)
        {
            return _proxyFactory.GetInterfaceToProxy(type, Configuration) != null;
        }
    }
}