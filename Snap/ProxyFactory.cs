using System;
using System.Linq;
using Castle.DynamicProxy;
using Fasterflect;

namespace Snap
{
    /// <summary>
    /// Incapsulates proxy creation logic, delegates actual proxy creation to ProxyGenerator of DynamicProxy2 assembly
    /// </summary>
    public class ProxyFactory
    {
        private readonly ProxyGenerator _proxyGenerator;

        public ProxyFactory(ProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public object CreateProxy(object instanceToProxy, IMasterProxy masterProxy)
        {
            object proxy;
            var interfaceToProxy = GetInterfaceToProxy(instanceToProxy.GetType(), masterProxy.Configuration);
            var interceptorsOfProxy = GetInterceptors(masterProxy);
            if(interfaceToProxy.IsInterface)
            {
                proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                    interfaceToProxy,
                    instanceToProxy,
                    interceptorsOfProxy);
            }
            else
            {
                var greediestCtor = interfaceToProxy.GetConstructors().OrderBy(x => x.Parameters().Count).LastOrDefault();
                var ctorDummyArgs =  greediestCtor == null 
                                     ? new object[0]
                                     : greediestCtor.Parameters()
                                       .Select(p => masterProxy.Container.GetInstance(p.ParameterType))
                                       .ToArray();

                proxy = _proxyGenerator.CreateClassProxyWithTarget(
                    interfaceToProxy,
                    instanceToProxy,
                    ctorDummyArgs, 
                    interceptorsOfProxy);
            }
            return proxy;
        }
        
        /// <summary>
        /// Determines the type of DynamicProxy that will be created over the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public Type GetInterfaceToProxy(Type type, AspectConfiguration configuration)
        {
            var allInterfaces = type.GetInterfaces();

            var baseClassInterfaces = type.BaseType != null ? type.BaseType.GetInterfaces() : new Type[0];
            var topLevelInterfaces = allInterfaces.Except(baseClassInterfaces);

            var levelInterfaces = topLevelInterfaces as Type[] ?? topLevelInterfaces.ToArray();
            if (!levelInterfaces.Any())
            {
                var types = new[] { type };
                return types.FirstMatch(configuration.Namespaces);
            }

            return levelInterfaces.ToArray().FirstMatch(configuration.Namespaces);
        }

        private static IInterceptor[] GetInterceptors(IMasterProxy masterProxy)
        {
            var pseudoInterceptors = Enumerable.Range(1, masterProxy.Configuration.Interceptors.Count)
                .Select(i => new PseudoInterceptor()).ToList();
            var lastInterceptor = pseudoInterceptors.LastOrDefault();

            var interceptors = new IInterceptor[] { masterProxy }
                .Concat(pseudoInterceptors)
                //.Cast<IInterceptor>()
                .ToArray();

            FlagLastInterceptor(lastInterceptor);

            masterProxy.ResetPseudoInterceptors = () =>
                                                      {
                                                          pseudoInterceptors.ForEach(pi => pi.ShouldProceed = false);
                                                          FlagLastInterceptor(lastInterceptor);
                                                      };

            return interceptors;
        }

        private static void FlagLastInterceptor(PseudoInterceptor lastInterceptor)
        {
            if (lastInterceptor != null)
            {
                lastInterceptor.ShouldProceed = true;
            }
        }
    }
}