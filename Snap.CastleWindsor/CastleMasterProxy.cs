using System.Collections.Generic;
using Castle.Core.Interceptor;

namespace Snap.CastleWindsor
{
    public class CastleMasterProxy : IInterceptor
    {
        private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public AspectConfiguration Configuration { get; set; }

        /// <summary>
        /// Intercepts the method for the proxied type.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            // Set the bound attribute type for each interceptor
            _interceptors.ForEach(i =>
                          {
                              if (i is IAttributeInterceptor && Configuration.Bindings[i.GetType()] != null)
                              {
                                  (i as IAttributeInterceptor).TargetAttribute =
                                      Configuration.Bindings[i.GetType()];
                              }
                          });

            // Invoke each interceptor
            _interceptors.ForEach(i => i.Intercept(invocation));
        }
        /// <summary>
        /// Adds an interceptor.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        public void AddInterceptor(IInterceptor interceptor)
        {
            _interceptors.Add(interceptor);
        }
    }
}
