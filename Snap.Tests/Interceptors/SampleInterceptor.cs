using System;
using System.Reflection;
using Castle.Core.Interceptor;

namespace Snap.Tests.Interceptors
{
    public class SampleInterceptor : MethodInterceptor
    {
        public SampleInterceptor() : base(typeof(SampleAttribute))
        {
        }

        public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
        {
            var start = DateTime.Now;
            invocation.Proceed();
            var end = DateTime.Now;

            Console.WriteLine(end - start);
        }
    }
}
