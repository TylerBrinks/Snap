using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;

namespace Snap.Autofac
{
    public class AutofacAspectModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Activating += RegistrationActivating;
        }

        private static void RegistrationActivating(object sender, ActivatingEventArgs<object> e)
        {
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

            e.Instance = new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(
                targetInterface, e.Instance, interceptors.ToArray());
        }
    }
}
