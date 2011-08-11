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
}