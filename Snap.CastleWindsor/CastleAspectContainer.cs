using System;
using System.Collections.Generic;
using Castle.Core.Interceptor;
using Castle.MicroKernel;

namespace Snap.CastleWindsor
{
    /// <summary>
    /// Castle Windsor Aspect Container for AoP interception registration.
    /// </summary>
    public class CastleAspectContainer : IAspectContainer
    {
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleAspectContainer"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CastleAspectContainer(IKernel container)
        {
            InterceptorTypes = new List<Type>();
            Proxy = new CastleMasterProxy();
            _kernel = container;
            _kernel.AddComponentInstance("CastleAspectContainer", this);
            _kernel.AddFacility<CastleAspectFacility>();
            _kernel.AddComponentInstance("cw", Proxy);
        }

        /// <summary>
        /// Gets or sets the interceptor types.
        /// </summary>
        /// <value>The interceptor types.</value>
        internal List<Type> InterceptorTypes { get; set; }
        /// <summary>
        /// Gets or sets the master proxy used for intercepting Castle-based instances.
        /// </summary>
        /// <value>The proxy.</value>
        internal CastleMasterProxy Proxy { get; set; }
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        internal AspectConfiguration Configuration { get; set; }

        /// <summary>
        /// Sets the container's configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        public void SetConfiguration(AspectConfiguration config)
        {
            config.Container = this;
            Configuration = config;
            Proxy.Configuration = config;
        }
        /// <summary>
        /// Binds an interceptor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Bind<T>() where T : IInterceptor, new()
        {
            InterceptorTypes.Add(typeof (T));
            Proxy.AddInterceptor(new T());
        }
    }
}
