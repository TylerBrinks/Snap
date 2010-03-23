using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;

namespace Snap.CastleWindsor
{
    /// <summary>
    /// Facility for Castle interceptor registration
    /// </summary>
    public class CastleAspectFacility : AbstractFacility
    {
        private CastleAspectContainer _container;

        /// <summary>
        /// Initializes the facility.
        /// </summary>
        protected override void Init()
        {
            _container = (CastleAspectContainer)Kernel["CastleWindsorAspectContainer"];
            Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }
        /// <summary>
        /// Kernel_s the component registered.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            _container.Handlers.ForEach(h => handler.ComponentModel.Interceptors.AddIfNotInCollection(new InterceptorReference(h)));
        }
    }
}
