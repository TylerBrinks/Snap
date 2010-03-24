using System;
using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// Defines a mapping between an interceptor and the attribute the interceptor responds to.
    /// </summary>
    public interface IAttributeInterceptor : IInterceptor
    {
        void BeforeInvocation();
        void AfterInvocation();
        void Invoke();
        Type TargetAttribute { get; set; }
    }
}
