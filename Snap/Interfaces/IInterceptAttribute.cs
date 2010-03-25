
namespace Snap
{
    /// <summary>
    /// IInterceptAttribute for targeting attribute-based interception.
    /// </summary>
    public interface IInterceptAttribute
    {
        int Order { get; set; }
    }
}
