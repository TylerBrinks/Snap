using System.Collections.Generic;

namespace Snap
{
    /// <summary>
    /// Sorts interceptors in their default configured order.
    /// </summary>
    public class DefaultSortOrderStrategy : ISortOrderStrategy
    {
        private readonly List<IAttributeInterceptor> _interceptors;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSortOrderStrategy"/> class.
        /// </summary>
        /// <param name="interceptors">The interceptors.</param>
        public DefaultSortOrderStrategy(List<IAttributeInterceptor> interceptors)
        {
            _interceptors = interceptors;
        }

        /// <summary>
        /// Sorts interceptors in the configured order.
        /// </summary>
        /// <returns>Sorted interceptors</returns>
        public List<IAttributeInterceptor> Sort()
        {
            return _interceptors;
        }
    }
}
