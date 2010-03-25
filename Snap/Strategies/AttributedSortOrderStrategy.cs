using System.Collections.Generic;
using System.Linq;

namespace Snap
{
    /// <summary>
    /// Sorts interceptors by attribute Order values.
    /// </summary>
    public class AttributedSortOrderStrategy : ISortOrderStrategy
    {
        private readonly List<IAttributeInterceptor> _interceptors;
        private readonly List<IInterceptAttribute> _attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributedSortOrderStrategy"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="interceptors">The interceptors.</param>
        public AttributedSortOrderStrategy(List<IInterceptAttribute> attributes, List<IAttributeInterceptor> interceptors)
        {
            _interceptors = interceptors;
            _attributes = attributes;
        }

        /// <summary>
        /// Sorts interceptors by attribute order, then by name.
        /// </summary>
        /// <returns>Sorted interceptors</returns>
        public List<IAttributeInterceptor> Sort()
        {
            var orderedInterceptors = new List<IAttributeInterceptor>();

            foreach (var attribute in _attributes.OrderBy(a => a.Order).ThenBy(a => a.GetType().Name))
            {
                var type = attribute.GetType();
                orderedInterceptors.Add(_interceptors.Where(i => i.TargetAttribute == type).First());
            }

            return orderedInterceptors;
        }
    }
}
