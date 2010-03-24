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
        
        public CastleAspectContainer(IKernel container)
        {
            Handlers = new List<Type>();

            _kernel = container;
            _kernel.AddComponentInstance("CastleWindsorAspectContainer", this);
            _kernel.AddFacility<CastleAspectFacility>();
        }

        internal List<Type> Handlers { get; set; }

        internal AspectConfiguration Configuration { get; set; }

        public void SetConfiguration(AspectConfiguration config)
        {
            config.Container = this;
            Configuration = config;
        }

        public void RegisterInterceptor<T>() where T : IInterceptor, new()
        {
            Handlers.Add(typeof(T));
            _kernel.AddComponent<T>();
        }
    }
}
