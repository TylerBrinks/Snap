using StructureMap;

namespace Snap.StructureMap
{
    /// <summary>
    /// StructureMap Aspect Container for AoP interception registration.
    /// </summary>
    public class StructureMapAspectContainer : AspectContainer
    {
        private readonly StructureMapAspectInterceptor _interceptor = new StructureMapAspectInterceptor();

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapAspectContainer"/> class.
        /// </summary>
        public StructureMapAspectContainer()
        {
            Proxy = new MasterProxy();

            // Call configure, not initialize.  Initialize overwrites existing settings.
            ObjectFactory.Configure(c =>
                                        {
                                            c.RegisterInterceptor(_interceptor);
                                            c.For<IMasterProxy>().Use(this.Proxy);
                                        });
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        public override void SetConfiguration(AspectConfiguration config)
        {
            _interceptor.Configuration = config;
            Proxy.Configuration = config;
            config.Container = this;
        }
    }
}
