using Ninject;

namespace Snap.Ninject
{
    /// <summary>
    /// Ninject Aspect Container for AoP interception registration.
    /// </summary>
    public class NinjectAspectContainer : AspectContainer
    {
        private readonly NinjectAspectInterceptor _interceptor = new NinjectAspectInterceptor();
        private readonly StandardKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectAspectContainer"/> class.
        /// </summary>
        public NinjectAspectContainer()
        {
            Proxy = new MasterProxy();
            _kernel = new StandardKernel(_interceptor);
        }

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public StandardKernel Kernel
        {
            get { return _kernel; }
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        public override void SetConfiguration(AspectConfiguration config)
        {
            Proxy.Configuration = config;
            _kernel.Bind<IMasterProxy>().ToConstant(Proxy);

            config.Container = this;
        }
    }
}
