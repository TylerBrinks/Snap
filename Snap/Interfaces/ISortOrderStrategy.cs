using System.Collections.Generic;

namespace Snap
{
    /// <summary>
    /// Defines a strategy for sorting attributes and interceptors.
    /// </summary>
    public interface ISortOrderStrategy
    {
        /// <summary>
        /// Sorts the instance.
        /// </summary>
        /// <returns>Sorted list.</returns>
        List<IAttributeInterceptor> Sort();
    }
}
