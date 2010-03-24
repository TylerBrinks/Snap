using System;
using System.Reflection;
using Castle.Core.Interceptor;
using Snap.Tests.Fakes;

namespace Snap.Tests.Interceptors
{
    public class SecondInterceptor : MethodInterceptor
    {
        public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
        {
            OrderedCode.Actions.Add("Second");
            invocation.Proceed();
        }
    }
}
