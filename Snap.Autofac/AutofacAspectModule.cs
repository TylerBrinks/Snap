using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Castle.Core.Interceptor;
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

            var pseudoList = new IInterceptor[proxy.Configuration.Interceptors.Count];
            pseudoList[0] = proxy;

            for (var i = 1; i < pseudoList.Length; i++)
            {
                pseudoList[i] = new PseudoInterceptor();
            }

            var interfaceTypes = e.Instance.GetType().GetInterfaces();
            var targetInterface = interfaceTypes.FirstOrDefault(i => proxy.Configuration.Namespaces.Any(n => i.FullName.Contains(n)));
            
            e.Instance = new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(targetInterface, e.Instance, pseudoList);
        }
    }
}
