using System;
using Castle.Core.Interceptor;

namespace Snap
{
    public interface IAttributeInterceptor : IInterceptor
    {
        Type TargetAttribute { get; set; }
    }
}
