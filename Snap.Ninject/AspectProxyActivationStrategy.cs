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
                        .FirstOrDefault(i => proxy.Configuration.Namespaces.Any(n => i.FullName.Contains(n)));

                    reference.Instance = AspectUtility.CreatePseudoProxy(proxy, targetInterface, reference.Instance);
                }
            }

            base.Activate(context, reference);
        }
    }
}
