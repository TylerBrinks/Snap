using System.Linq;
using Castle.Core.Interceptor;

namespace Snap
{
    public class MasterProxy : IMasterProxy
    {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public AspectConfiguration Configuration
        {
            get;
            set;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            var interceptors = Configuration.Interceptors.ToList();

            var sortOrder = SortOrderFactory.GetSortOrderStrategy(invocation, interceptors);

            var orderedInterceptors = sortOrder.Sort();

            foreach (var interceptor in orderedInterceptors)
            {
                interceptor.BeforeInvocation();
                interceptor.Intercept(invocation);
                interceptor.AfterInvocation();
            }
        }
    }
}
