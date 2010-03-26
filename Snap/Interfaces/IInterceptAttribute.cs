
namespace Snap
{
    /// <summary>
    /// IInterceptAttribute for targeting attribute-based interception.
    /// </summary>
    public interface IInterceptAttribute
    {
        /// <summary>
        /// Gets or sets the execution order.
        /// </summary>
        /// <value>The execution order.</value>
        int Order { get; set; }
    }
}
