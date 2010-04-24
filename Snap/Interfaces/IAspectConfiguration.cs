
namespace Snap
{
    /// <summary>
    /// Defines syntax for configuring aspects
    /// </summary>
    public interface IAspectConfiguration : IHideBaseTypes
    {
        /// <summary>
        /// Creates an interceptor binding.
        /// </summary>
        /// <typeparam name="T">Type of interceptor</typeparam>
        /// <returns>IConfigurationSyntax instance</returns>
        IConfigurationSyntax Bind<T>() where T : IAttributeInterceptor, new();
        /// <summary>
        /// Includes a namespace for type interception.
        /// </summary>
        /// <param name="name">The namespace.</param>
        void IncludeNamespace(string name);

        /// <summary>
        /// Includes a namespace root for type interception.
        /// </summary>
        /// <param name="namePrefix">The name prefix.</param>
        void IncludeNamespaceRoot(string namePrefix);
    }
}
