using Castle.Core.Interceptor;
using StructureMap;

namespace Snap.StructureMap
{
    /// <summary>
    /// StructureMap Aspect Container for AoP interception registration.
    /// </summary>
    public class StructureMapAspectContainer : IAspectContainer
    {
        private readonly StructureMapAspectInterceptor _interceptor = new StructureMapAspectInterceptor();

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapAspectContainer"/> class.
        /// </summary>
        public StructureMapAspectContainer()
        {
            ObjectFactory.Configure(c => c.RegisterInterceptor(_interceptor));
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        public void SetConfiguration(AspectConfiguration config)
        {
            _interceptor.Configuration = config;
            config.Container = this;
        }
        /// <summary>
        /// Registers the interceptor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RegisterInterceptor<T>() where T : IInterceptor, new()
        {
            // Call configure, not initialize.  Initialize overwrites existing settings.
            ObjectFactory.Configure(c => c.For<IInterceptor>().Use(new T()));
        }
    }
}
