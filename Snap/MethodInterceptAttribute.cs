using System;

namespace Snap
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public class MethodInterceptAttribute : Attribute, IInterceptAttribute
    {
    }
}
