using Ninject.Activation.Strategies;
using Ninject.Modules;

namespace Snap.Ninject
{
    /// <summary>
    /// Ninject module for intercepting methods.
    /// </summary>
    public class NinjectAspectInterceptor : NinjectModule
    {
        public override void Load()
        {
            Kernel.Components.Add<IActivationStrategy, AspectProxyActivationStrategy>();
        }
    }
}
