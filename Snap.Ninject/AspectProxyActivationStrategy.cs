using System;
using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
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
        private Type _targetInterface;

        internal AspectConfiguration Configuration { get;set; }
        
        /// <summary>
        /// Creates and wraps the reference type in a Castle proxy
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference.</param>
        public override void Activate(IContext context, InstanceReference reference)
        {
            if (reference.Instance as IInterceptor == null && reference.Instance as INinjectAspectConfiguration == null)
            {
                Configuration = context.Kernel.Get<INinjectAspectConfiguration>().Configuration;

                QueryTargetType(reference.Instance.GetType());
                var interceptors = context.Kernel.GetAll<IInterceptor>();

                reference.Instance = AspectUtility.CreateProxy(_targetInterface, reference.Instance, interceptors.ToArray());
            }

            base.Activate(context, reference);
        }
        /// <summary>
        /// Queries the target type for implementation of a given interface
        /// </summary>
        /// <param name="type">The type to query.</param>
        /// <returns>List of implemented interfaces.</returns>
        public Type[] QueryTargetType(Type type)
        {
            var interfaceTypes = type.GetInterfaces();

            var namespaces = Configuration.Namespaces;

            // Filter the interfaces by given namespaces that implement IInterceptAspect
            _targetInterface = interfaceTypes.FirstOrDefault(i => namespaces.Any(n => i.FullName.Contains(n)) &&
                i.FullName != "Snap.IInterceptAspect");

            return interfaceTypes;
        }
    }
}
