using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Castle.DynamicProxy;
using SnapTests.Fakes;

namespace Snap.Tests.Interceptors
{
    public class FourthClassInterceptor : MethodInterceptor
    {
        private readonly string _customAction;

        public FourthClassInterceptor()
            : this("Fourth")
        { }

        public FourthClassInterceptor(string customAction)
        {
            _customAction = customAction;
        }

        public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
        {
            OrderedCode.Actions.Add(_customAction);
            invocation.Proceed();
        }
    }
}
