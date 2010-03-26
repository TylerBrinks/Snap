using System;
using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using Fasterflect;

namespace Snap
{
    /// <summary>
    /// Utility methods
    /// </summary>
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
        /// <summary>
        /// Determines whether the specified target object has methods decorated for interception.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// 	<c>true</c> if the specified target is decorated; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDecorated(this object target, AspectConfiguration configuration)
        {
            var methods = target.GetType().Methods();

            var isDecorated = methods.Any(m => m.Attributes().Any(a => a is MethodInterceptAttribute));

            return isDecorated;
        }
    }
}
