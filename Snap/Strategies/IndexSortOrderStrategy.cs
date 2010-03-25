using System.Linq;
using System.Collections.Generic;

namespace Snap
{
    /// <summary>
    /// Sorts interceptors by their configured order index.
    /// </summary>
    public class IndexSortOrderStrategy : ISortOrderStrategy
    {
        private readonly List<IAttributeInterceptor> _interceptors;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexSortOrderStrategy"/> class.
        /// </summary>
        /// <param name="interceptors">The interceptors.</param>
        public IndexSortOrderStrategy(List<IAttributeInterceptor> interceptors)
        {
            _interceptors = interceptors;
        }

        /// <summary>
        /// Sorts interceptors by their configured order index, then by attribute name.
        /// </summary>
        /// <returns></returns>
        public List<IAttributeInterceptor> Sort()
        {
            return _interceptors.OrderBy(i => i.Order).ThenBy(i => i.TargetAttribute.Name).ToList();
        }
    }
}
