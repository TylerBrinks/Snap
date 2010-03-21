using System;
using System.Reflection;
using Castle.Core.Interceptor;

namespace Snap.Tests.Interceptors
{
    public class HandleErrorInterceptor : MethodInterceptor
    {
        public HandleErrorInterceptor()
            : base(typeof(HandleErrorAttribute))
        {
        }

        public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
        {
            try
            {
                // Attempt the method call
                invocation.Proceed();
            }
            catch
            {
                // Do something useful here.
            }
        }
    }
}
