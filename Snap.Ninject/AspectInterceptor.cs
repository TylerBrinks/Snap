using Ninject.Activation.Strategies;
using Ninject.Modules;

namespace Snap.Ninject
{
    /// <summary>
    /// Ninject module for intercepting methods.
    /// </summary>
    public class AspectInterceptor : NinjectModule
    {
        public override void Load()
        {
            Kernel.Components.Add<IActivationStrategy, AspectProxyActivationStrategy>();
        }
    }
}
