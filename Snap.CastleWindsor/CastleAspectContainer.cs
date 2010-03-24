using Castle.MicroKernel;

namespace Snap.CastleWindsor
{
    /// <summary>
    /// Castle Windsor Aspect Container for AoP interception registration.
    /// </summary>
    public class CastleAspectContainer : AspectContainer
    {
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleAspectContainer"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CastleAspectContainer(IKernel container)
        {
            //InterceptorTypes = new List<Type>();
            Proxy = new MasterProxy();
            _kernel = container;
            _kernel.AddComponentInstance("CastleAspectContainer", this);
            _kernel.AddFacility<CastleAspectFacility>();
            _kernel.AddComponentInstance("Proxy", Proxy);
        }

        /// <summary>
        /// Sets the container's configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        public override void SetConfiguration(AspectConfiguration config)
        {
            config.Container = this;
            Proxy.Configuration = config;
        }
    }
}
