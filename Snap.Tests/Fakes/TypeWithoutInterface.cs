using System;
using Snap.Tests.Interceptors;

namespace SnapTests.Fakes
{
    public class TypeWithoutInterface
    {
        [HandleError]
        public virtual void Foo()
        {
            throw new Exception("throws unless swallowed by interceptor");
        }
    }

    public class TypeWithInterfaceInBaseClass : BaseClassWithInterface
    {
        [HandleError]
        public virtual void Foo()
        {
            throw new Exception("throws unless swallowed by interceptor");
        }
    }

    public class BaseClassWithInterface : IBadCode
    {
        public void GiddyUp()
        {
            throw new NotImplementedException();
        }
    }
}