
namespace Snap
{
    /// <summary>
    /// Extends syntax for aspect configuration.  This is where the fluent configuration
    /// really begins.
    /// </summary>
    public interface IConfigurationSyntax : IHideBaseTypes
    {
        /// <summary>
        /// Binds an attribute to an interceptor.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns>Order index syntax</returns>
        IOrderSyntax To<TAttribute>();
    }
}
