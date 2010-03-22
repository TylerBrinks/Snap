
using System;
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
    }
}
