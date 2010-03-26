
namespace Snap
{
    /// <summary>
    /// Extends syntax for index ordering configuration.
    /// </summary>
    public interface IOrderSyntax : IHideBaseTypes
    {
        /// <summary>
        /// Orders the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        void Order(int index);
    }
}
