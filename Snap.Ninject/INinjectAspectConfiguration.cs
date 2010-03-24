
namespace Snap.Ninject
{
    /// <summary>
    /// Ninject-specific aspect configuration
    /// </summary>
    public interface INinjectAspectConfiguration
    {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        AspectConfiguration Configuration { get; set; }
    }
}
