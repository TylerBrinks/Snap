using System;
using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using StructureMap;
using StructureMap.Interceptors;

namespace Snap.StructureMap
{
    /// <summary>
    /// StructureMap type interceptor
    /// </summary>
    public class StructureMapAspectInterceptor : TypeInterceptor
    {
        private Type _targetInterface;

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        internal AspectConfiguration Configuration { get; set; }

        /// <summary>
        /// Wraps interfaces in a Castle dynamic proxy
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public object Process(object target, IContext context)
        {
            var interceptors = context.GetAllInstances<IInterceptor>();

            AspectUtility.SetTargetAttributeTypes(interceptors, Configuration);

            QueryTargetType(target.GetType());

            return new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(_targetInterface, target, interceptors.ToArray());
        }
        /// <summary>
        /// Matcheses types in the a namespace that implement IInterceptAspect.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool MatchesType(Type type)
        {
            // Get all the type's interfaces
            /*var interfaceTypes =*/ QueryTargetType(type);
            
            // Ignore all external types (i.e. System.Web...)
            return _targetInterface != null;// && interfaceTypes.Any(i => i.Name == "IInterceptAspect");
        }
        /// <summary>
        /// Queries the target type for implementation of a given interface
        /// </summary>
        /// <param name="type">The type to query.</param>
        /// <returns>List of implemented interfaces.</returns>
        public Type[] QueryTargetType(Type type)
        {
            var interfaceTypes = type.GetInterfaces();

            var namespaces = Configuration.Namespaces;

             // Filter the interfaces by given namespaces that implement IInterceptAspect
             _targetInterface = interfaceTypes.FirstOrDefault(i => namespaces.Any(n => i.FullName.Contains(n)) &&
                 i.FullName != "Snap.IInterceptAspect");

            return interfaceTypes;
        }
    }
}