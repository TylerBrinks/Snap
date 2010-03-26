using Castle.Core.Interceptor;

namespace Snap
{
    public interface IMasterProxy : IInterceptor, IHideBaseTypes
    {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        AspectConfiguration Configuration { get; set; }
    }
}
