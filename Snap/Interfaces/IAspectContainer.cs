
namespace Snap
{
    /// <summary>
    /// Defines an AoP container
    /// </summary>
    public interface IAspectContainer : IHideBaseTypes
    {
        /// <summary>
        /// Sets the container's configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        void SetConfiguration(AspectConfiguration config);
        /// <summary>
        /// Gets or sets the master proxy.
        /// </summary>
        /// <value>The master proxy.</value>
        IMasterProxy Proxy { get; set; }
    }
}
