using Microsoft.Practices.ServiceLocation;

namespace Snap
{
    /// <summary>
    /// Defines strategy to create interceptor instances
    /// Creates interceptor resolving it from IoC container. 
    /// IoC container is injected using CommonServiceLocator concept in a container-agnostic manner.
    /// </summary>
    public class ResolveInterceptorFromContainerCreationStrategy : IInterceptorCreationStrategy
    {
        private readonly IServiceLocator _container;

        public ResolveInterceptorFromContainerCreationStrategy(IServiceLocator container)
        {
            _container = container;
        }

        /// <summary>
        /// Creates instance of <paramref name="interceptorRegistration">interceptor</paramref> for given interceptor registration.
        /// </summary>
        /// <param name="interceptorRegistration">Registration of interceptor, is built on aspect configuration build step</param>
        /// <returns></returns>
        public IAttributeInterceptor Create(InterceptorRegistration interceptorRegistration)
        {
            // Assume interceptor type implements IAttributeInterceptor contract. 
            // This rule is enforced on aspect configuration build step
            // Create new instance by means of constructor. Switch using service locator later.
            var interceptor = (IAttributeInterceptor) _container.GetInstance(interceptorRegistration.InterceptorType);

            // reassign target attribute and order properties from interception registration to actual interceptor instance
            interceptor.TargetAttribute = interceptorRegistration.TargetAttributeType;
            interceptor.Order = interceptorRegistration.Order;
            return interceptor;
        }
    }
}