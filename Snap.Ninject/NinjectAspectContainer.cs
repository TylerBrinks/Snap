using Castle.Core.Interceptor;
using Ninject;

namespace Snap.Ninject
{
    /// <summary>
    /// Ninject Aspect Container for AoP interception registration.
    /// </summary>
    public class NinjectAspectContainer : IAspectContainer
    {
        private readonly NinjectAspectInterceptor _interceptor = new NinjectAspectInterceptor();
        private readonly StandardKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectAspectContainer"/> class.
        /// </summary>
        public NinjectAspectContainer()
        {
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
        public void SetConfiguration(AspectConfiguration config)
        {
            _kernel.Bind<INinjectAspectConfiguration>()
                .ToConstant(new NinjectAspectConfiguration { Configuration = config });

            config.Container = this;
        }
        /// <summary>
        /// Registers an interceptor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Bind<T>() where T : IInterceptor, new()
        {
            _kernel.Bind<IInterceptor>().To<T>();
        }
    }
}
