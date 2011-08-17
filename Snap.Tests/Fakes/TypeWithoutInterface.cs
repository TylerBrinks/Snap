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
        private readonly IDependency _dependency;

        public TypeWithInterfaceInBaseClass(IDependency dependency)
        {
            _dependency = dependency;
        }

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