using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// Defines an AoP container
    /// </summary>
    public interface IAspectContainer
    {
        void SetConfiguration(AspectConfiguration config);
        void RegisterInterceptor<T>() where T : IInterceptor, new();
    }
}
