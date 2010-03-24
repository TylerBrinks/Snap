using System;
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
        /// Creates a proxy around an instance with pseudo (empty) interceptors.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="instanceToWrap">The instance to wrap.</param>
        /// <returns></returns>
        public static object CreatePseudoProxy(IMasterProxy proxy, Type interfaceType, object instanceToWrap)
        {
            var pseudoList = new IInterceptor[proxy.Configuration.Interceptors.Count];
            pseudoList[0] = proxy;

            for (var i = 1; i < pseudoList.Length; i++)
            {
                pseudoList[i] = new PseudoInterceptor();
            }

            return CreateProxy(interfaceType, instanceToWrap, pseudoList);
        }
    }
}
