using Ninject.Activation.Strategies;
using Ninject.Modules;

namespace Snap.Ninject
{
    /// <summary>
    /// Ninject module for intercepting methods.
    /// </summary>
    public class NinjectAspectInterceptor : NinjectModule
    {
        /// <summary>
        /// Loads the AspectProxyActivationStrategy into the kernel's component list.
        /// </summary>
        public override void Load()
        {
            Kernel.Components.Add<IActivationStrategy, AspectProxyActivationStrategy>();
        }
    }
}
