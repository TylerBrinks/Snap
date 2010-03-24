using System;
using System.Collections.Generic;
using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// Provider-agnostic Aspect-Oriented configuration
    /// </summary>
    public class AspectConfiguration : IAspectConfiguration
    {
        private readonly List<string> _namespaces = new List<string>();
        private readonly Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();

        /// <summary>
        /// Registers a method interceptor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public IConfigurationSyntax Bind<T>() where T : IInterceptor, new()
        {
            Container.Bind<T>();
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
        /// Gets the bindings.
        /// </summary>
        /// <value>The bindings.</value>
        public Dictionary<Type, Type> Bindings
        {
            get { return _bindings; }
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
            _bindings.Add(typeof (T), typeof (TAttribute));
        }
    }
}