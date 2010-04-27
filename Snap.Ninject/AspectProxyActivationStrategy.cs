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
using System.Linq;
using Castle.Core.Interceptor;
using Ninject;
using Ninject.Activation;
using Ninject.Activation.Strategies;

namespace Snap.Ninject
{
    /// <summary>
    /// Ninject Type creation strategy.
    /// </summary>
    public class AspectProxyActivationStrategy : ActivationStrategy
    {
        /// <summary>
        /// Creates and wraps the reference type in a Castle proxy
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The instance reference.</param>
        public override void Activate(IContext context, InstanceReference reference)
        {
            // Don't try to IInterceptor or MasterProxy instances.
            if (reference.Instance as IInterceptor == null && reference.Instance.GetType() != typeof(MasterProxy)) // as INinjectAspectConfiguration == null)
            {
                var proxy = context.Kernel.Get<IMasterProxy>();

                // Only build a proxy for decorated types
                if (reference.Instance.IsDecorated(proxy.Configuration))
                {
                    var type = reference.Instance.GetType();

                    // Filter the interfaces by given namespaces that implement IInterceptAspect
                    var targetInterface = type.GetInterfaces()
                        .FirstMatch(proxy.Configuration.Namespaces);
                        //.FirstOrDefault(i => proxy.Configuration.Namespaces.Any(n => i.FullName.IsMatch(n)));

                    reference.Instance = AspectUtility.CreatePseudoProxy(proxy, targetInterface, reference.Instance);
                }
            }

            base.Activate(context, reference);
        }
    }
}
