
namespace Snap
{
    /// <summary>
    /// Extends syntax for index ordering configuration.
    /// </summary>
    public interface IOrderSyntax : IHideBaseTypes
    {
        void Order(int index);
    }
}
