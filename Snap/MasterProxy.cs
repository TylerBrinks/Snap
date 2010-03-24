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

            // TODO: Reorder the list here based on config - default to 

            foreach (var interceptor in interceptors)
            {
                interceptor.BeforeInvocation();
                interceptor.Intercept(invocation);
                interceptor.AfterInvocation();
            }
        }
    }
}
