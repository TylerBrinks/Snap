using System;
using System.Linq;
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
            var proxy = (MasterProxy)ObjectFactory.GetInstance<IMasterProxy>();

            QueryTargetType(target.GetType());

            if (target.IsDecorated(proxy.Configuration))
            {
                return AspectUtility.CreatePseudoProxy(proxy, _targetInterface, target);
            }

            var name = target.GetType().FullName;
            // Don't build up any wrapped proxy types.
            if (!(name.StartsWith("Castle.Proxies.") && name.EndsWith("Proxy")))
            {
                context.BuildUp(target);
            }

            return target;
        }
        /// <summary>
        /// Matcheses types in the a namespace that implement IInterceptAspect.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool MatchesType(Type type)
        {
            QueryTargetType(type);

            return _targetInterface != null;
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
            //_targetInterface = interfaceTypes.FirstOrDefault(i => namespaces.Any(n => i.FullName.IsMatch(n)));
            _targetInterface = interfaceTypes.FirstMatch(namespaces);

            return interfaceTypes;
        }
    }
}