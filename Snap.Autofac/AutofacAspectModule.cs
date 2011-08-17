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

using Autofac;
using Autofac.Core;
using Castle.DynamicProxy;

namespace Snap.Autofac
{
    /// <summary>
    /// Autofac module for intercepting types during creation.
    /// </summary>
    public class AutofacAspectModule : Module
    {
        /// <summary>
        /// Attaches to component registration.
        /// </summary>
        /// <param name="componentRegistry">The component registry.</param>
        /// <param name="registration">The registration.</param>
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Activating += RegistrationActivating;
        }

        /// <summary>
        /// Registrations the activating.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Autofac.Core.ActivatingEventArgs&lt;System.Object&gt;"/> instance containing the event data.</param>
        private static void RegistrationActivating(object sender, ActivatingEventArgs<object> e)
        {
            // Ignore AspectConfiguration and IInterceptor types since they're being referenced via the Autofac
            // registration context. Otherwise calling e.Context.Resolve<IInterceptor> will fail when
            // the code below executes.
            if (e.Instance.GetType() == typeof (MasterProxy) || e.Instance is IInterceptor)
            {
                return;
            }

            var proxy = (MasterProxy)e.Context.Resolve(typeof(MasterProxy));

            if (!e.Instance.IsDecorated(proxy.Configuration))
            {
                return;
            }

            var pseudoList = new IInterceptor[proxy.Configuration.Interceptors.Count];
            pseudoList[0] = proxy;

            for (var i = 1; i < pseudoList.Length; i++)
            {
                pseudoList[i] = new PseudoInterceptor();
            }

            var targetInterface = e.Instance.GetType().GetTypeToDynamicProxy(proxy.Configuration.Namespaces);

            e.Instance = AspectUtility.CreateProxy(targetInterface, e.Instance, pseudoList);
        }
    }
}
