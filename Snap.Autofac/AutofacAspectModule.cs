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
            if (e.Instance.GetType() == typeof (AspectConfiguration) || e.Instance is IInterceptor)
            {
                return;
            }

            var components = e.Context.ComponentRegistry.Registrations
                .Where(r => r.Target.Services.Any(s => s.Description.Contains("IInterceptor")))
                .SelectMany(svc => svc.Services)
                    .Where(i => i.Description != "Castle.Core.Interceptor.IInterceptor")
                .Select(interceptor => interceptor);

            var config = e.Context.Resolve<AspectConfiguration>();
            var type = e.Instance.GetType();
            var interfaceTypes = type.GetInterfaces();

            var namespaces = config.Namespaces;

            // Filter the interfaces by given namespaces that implement IInterceptAspect
            var targetInterface = interfaceTypes.FirstOrDefault(i => namespaces.Any(n => i.FullName.Contains(n)) &&
                                                                     i.FullName != "Snap.IInterceptAspect");

            var interceptors = new List<IInterceptor>();

            components.ToList().ForEach(i => interceptors.Add(e.Context.Resolve(i) as IInterceptor));

            e.Instance = AspectUtility.CreateProxy(targetInterface, e.Instance, interceptors.ToArray());
        }
    }
}
