using System;
using System.Reflection;
using Castle.Core.Interceptor;
using Snap.Tests.Fakes;

namespace Snap.Tests.Interceptors
{
    public class ThirdInterceptor : MethodInterceptor
    {
        public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
        {
            OrderedCode.Actions.Add("Third");
            invocation.Proceed();
        }
    }
}
