
namespace Snap
{
    public abstract class AspectContainer : IAspectContainer
    {
        public abstract void SetConfiguration(AspectConfiguration config);

        /// <summary>
        /// Gets or sets the master proxy.
        /// </summary>
        /// <value>The master proxy.</value>
        public IMasterProxy Proxy { get; set; }
    }
}
