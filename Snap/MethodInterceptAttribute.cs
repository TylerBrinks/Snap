using System;

namespace Snap
{
    /// <summary>
    /// Used to decorate methods for interception.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public class MethodInterceptAttribute : Attribute, IInterceptAttribute
    {
    }
}
