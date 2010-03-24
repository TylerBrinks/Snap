using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;

namespace Snap
{
    public static class AspectUtility
    {
        public static object CreateProxy(Type interfaceType, object instanceToWrap, params IInterceptor[] interceptors)
        {
            return new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(interfaceType, instanceToWrap, interceptors.ToArray());
        }

        public static void SetTargetAttributeTypes(IEnumerable<IInterceptor> interceptors, AspectConfiguration configuration)
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
