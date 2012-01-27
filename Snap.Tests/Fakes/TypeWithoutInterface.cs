using System;
using Snap.Tests.Interceptors;
using Snap;

namespace SnapTests.Fakes
{
    public class TypeWithoutInterface
    {
        [HandleError]
        public virtual void Foo()
        {
            throw new Exception("throws unless swallowed by interceptor");
        }

        protected virtual void Foo2()
        {
            throw new Exception("throws unless swallowed by interceptor");
        }

        public virtual void CallFoo2()
        {
            Foo2();
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
    
    [PublicProtectedClass(MulticastOptions = MulticastOptions.Public | MulticastOptions.Protected)]
    public abstract class BaseAbstractClass
    {
        protected virtual void Foo2()
        {
            throw new Exception("throws unless swallowed by interceptor");
        }

        public abstract void CallFoo2();
    }

    [PublicClass(MulticastOptions = MulticastOptions.Public, ExcludePattern = "CallFoo2")]
    public class TypeWithAbstractClass : BaseAbstractClass
    {
        public override void CallFoo2()
        {
            Foo2();
        }
    }
}