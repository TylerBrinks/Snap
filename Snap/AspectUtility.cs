using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;

namespace Snap
{
    public static class AspectUtility
    {
        /// <summary>
        /// Creates a proxy around an instance with type interceptors.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="instanceToWrap">The instance to wrap.</param>
        /// <param name="interceptors">The interceptors.</param>
        /// <returns>Wrapped instance</returns>
        public static object CreateProxy(Type interfaceType, object instanceToWrap, params IInterceptor[] interceptors)
        {
            return new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(interfaceType, instanceToWrap, interceptors.ToArray());
        }

        /// <summary>
        /// Sets the configured attribute type bindings on a list of interceptors.
        /// </summary>
        /// <param name="interceptors">The interceptors.</param>
        /// <param name="configuration">The configuration.</param>
        public static void SetTargetAttributeTypes(IList<IInterceptor> interceptors, AspectConfiguration configuration)
        {
            foreach (var interceptor in interceptors)
            {
                var type = interceptor.GetType();
                if (configuration.Bindings[type] != null && interceptor is IAttributeInterceptor)
                {
                    (interceptor as IAttributeInterceptor).TargetAttribute = configuration.Bindings[type];
                }
            }
        }
    }
}
