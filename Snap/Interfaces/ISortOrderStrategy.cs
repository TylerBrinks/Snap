using System.Collections.Generic;

namespace Snap
{
    /// <summary>
    /// Defines a strategy for sorting attributes and interceptors.
    /// </summary>
    public interface ISortOrderStrategy
    {
        List<IAttributeInterceptor> Sort();
    }
}
