
namespace Snap
{
    /// <summary>
    /// Base container for aspect and interceptor type registration
    /// </summary>
    public abstract class AspectContainer : IAspectContainer
    {
        /// <summary>
        /// Sets the container's configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        public abstract void SetConfiguration(AspectConfiguration config);
        /// <summary>
        /// Gets or sets the master proxy.
        /// </summary>
        /// <value>The master proxy.</value>
        public IMasterProxy Proxy { get; set; }
    }
}
