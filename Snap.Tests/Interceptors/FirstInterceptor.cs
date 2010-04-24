using System;
using System.Reflection;
using Castle.Core.Interceptor;
using SnapTests.Fakes;

namespace Snap.Tests.Interceptors
{
    public class FirstInterceptor : MethodInterceptor
    {
        public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
        {
            OrderedCode.Actions.Add("First");
            invocation.Proceed();
        }
    }
}
