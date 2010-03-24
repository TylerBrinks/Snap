using Castle.Core.Interceptor;

namespace Snap
{
    public interface IMasterProxy : IInterceptor
    {
        AspectConfiguration Configuration { get; set; }
    }
}
