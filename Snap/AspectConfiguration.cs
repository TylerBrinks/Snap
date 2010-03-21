using System.Collections.Generic;
using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// Provider-agnostic Aspect-Oriented configuration
    /// </summary>
    public class AspectConfiguration
    {
        private readonly List<string> _namespaces = new List<string>();

        /// <summary>
        /// Registers a method interceptor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RegisterInterceptor<T>() where T : IInterceptor, new()
        {
            Container.RegisterInterceptor<T>();
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
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        public IAspectContainer Container { get; set; }
    }
}