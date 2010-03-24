using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// Defines an AoP container
    /// </summary>
    public interface IAspectContainer
    {
        /// <summary>
        /// Sets the container's configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        void SetConfiguration(AspectConfiguration config);
        /// <summary>
        /// Registers an interceptor.
        /// </summary>
        /// <typeparam name="T">Type of interceptor</typeparam>
        void Bind<T>() where T : IInterceptor, new();
    }
}
