using System.Linq;
using System.Collections.Generic;
using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// Creates sorting strategies.
    /// </summary>
    public static class SortOrderFactory
    {
        /// <summary>
        /// Gets the sort order strategy.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="interceptors">The interceptors.</param>
        /// <returns>Sort order strategy.</returns>
        public static ISortOrderStrategy GetSortOrderStrategy(IInvocation invocation, List<IAttributeInterceptor> interceptors)
        {
            var attributes = invocation.MethodInvocationTarget.GetCustomAttributes(false)
                .Where(a => a is IInterceptAttribute).Select(at => (IInterceptAttribute)at).ToList();

            var attributesAreIndexed = attributes.Any(a => a.Order > 0);
            var interceptorsAreIndexed = interceptors.Any(a => a.Order > 0);
        
            if(attributesAreIndexed)
            {
                return new AttributedSortOrderStrategy(attributes, interceptors);
            }
            
            if(interceptorsAreIndexed)
            {
                return new IndexSortOrderStrategy(interceptors);
            }

            return new DefaultSortOrderStrategy(interceptors);
        }
    }
}
