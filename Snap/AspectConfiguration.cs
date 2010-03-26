using System.Linq;
using System.Collections.Generic;

namespace Snap
{
    /// <summary>
    /// Provider-agnostic Aspect-Oriented configuration
    /// </summary>
    public class AspectConfiguration : IAspectConfiguration
    {
        private readonly List<string> _namespaces = new List<string>();
        private readonly List<IAttributeInterceptor> _interceptors = new List<IAttributeInterceptor>();

        /// <summary>
        /// Registers a method interceptor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public IConfigurationSyntax Bind<T>() where T : IAttributeInterceptor, new()
        {
            _interceptors.Add(new T());
            return new ConfigurationSyntax<T>(this);
        }
        /// <summary>
        /// Includes a namespace for AOP method interception type lookups.
        /// </summary>
        /// <param name="name">The namespace to include.</param>
        public void IncludeNamespace(string name)
        {
            if (!_namespaces.Contains(name))
            {
                _namespaces.Add(name);
            }
        }
        /// <summary>
        /// Gets the list of configured namespaces.
        /// </summary>
        /// <value>The namespace list.</value>
        public List<string> Namespaces
        {
            get { return _namespaces; }
        }
        /// <summary>
        /// Gets the interceptors.
        /// </summary>
        /// <value>The interceptors.</value>
        public List<IAttributeInterceptor> Interceptors
        {
            get { return _interceptors; }
        }
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        public IAspectContainer Container { get; set; }

        /// <summary>
        /// Binds an interceptor to an attribute.
        /// </summary>
        /// <typeparam name="T">Interceptor type.</typeparam>
        /// <typeparam name="TAttribute">The type of attribute.</typeparam>
        internal void BindInterceptor<T, TAttribute>()
        {
            _interceptors.Where(i => i.GetType() == typeof (T)).First().TargetAttribute = typeof (TAttribute);
        }
        /// <summary>
        /// Adds the binding order for an interceptor.
        /// </summary>
        /// <param name="index">The order index.</param>
        internal void AddBindingOrder(int index)
        {
            _interceptors.Last().Order = index;
        }
    }
}