using System;

namespace Snap
{
    /// <summary>
    /// Defines strategy to create interceptor instances
    /// Creates interceptor directly by calling <see cref="Activator.CreateInstance(System.Type)">>Activator.CreateInstance</see> method.
    /// </summary>
    public class InstantiateInterceptorDirectlyCreationStrategy : IInterceptorCreationStrategy
    {
        /// <summary>
        /// Creates instance of <paramref name="interceptorRegistration">interceptor</paramref> for given interceptor registration
        /// </summary>
        /// <param name="interceptorRegistration">Registration of interceptor, is built on aspect configuration build step</param>
        /// <returns></returns>
        public IAttributeInterceptor Create(InterceptorRegistration interceptorRegistration)
        {
            // Assume interceptor type implements IAttributeInterceptor contract and has parameterless constructor. 
            // This rule is enforced on aspect configuration build step
            // Create new instance by means of constructor. Switch using service locator later.
            var interceptor = (IAttributeInterceptor)Activator.CreateInstance(interceptorRegistration.InterceptorType);

            // reassign target attribute and order properties from interception registration to actual interceptor instance
            interceptor.TargetAttribute = interceptorRegistration.TargetAttributeType;
            interceptor.Order = interceptorRegistration.Order;

            return interceptor;
        }
    }
}