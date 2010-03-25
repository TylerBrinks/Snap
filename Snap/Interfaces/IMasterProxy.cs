using Castle.Core.Interceptor;

namespace Snap
{
    public interface IMasterProxy : IInterceptor, IHideBaseTypes
    {
        AspectConfiguration Configuration { get; set; }
    }
}
