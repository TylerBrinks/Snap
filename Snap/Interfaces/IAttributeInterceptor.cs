using System;
using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// Defines a mapping between an interceptor and the attribute the interceptor responds to.
    /// </summary>
    public interface IAttributeInterceptor : IInterceptor, IHideBaseTypes
    {
        void BeforeInvocation();
        void AfterInvocation();
        Type TargetAttribute { get; set; }
        int Order { get; set; }
    }
}
