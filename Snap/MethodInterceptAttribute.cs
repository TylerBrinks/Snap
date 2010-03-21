using System;

namespace Snap
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class MethodInterceptAttribute : Attribute
    {
    }
}
