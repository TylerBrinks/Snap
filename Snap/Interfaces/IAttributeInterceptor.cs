using System;
using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// Defines a mapping between an interceptor and the attribute the interceptor responds to.
    /// </summary>
    public interface IAttributeInterceptor : IInterceptor, IHideBaseTypes
    {
        /// <summary>
        /// Fired before the interceptor invocation.
        /// </summary>
        void BeforeInvocation();
        /// <summary>
        /// Fired after the interceptor invocation.
        /// </summary>
        void AfterInvocation();
        /// <summary>
        /// Gets or sets the target attribute.
        /// </summary>
        /// <value>The target attribute.</value>
        Type TargetAttribute { get; set; }
        /// <summary>
        /// Gets or sets the execution order.
        /// </summary>
        /// <value>The execution order.</value>
        int Order { get; set; }
    }
}
