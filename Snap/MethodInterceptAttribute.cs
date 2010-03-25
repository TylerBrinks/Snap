using System;

namespace Snap
{
    /// <summary>
    /// Used to decorate methods for interception.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public class MethodInterceptAttribute : Attribute, IInterceptAttribute
    {
        public MethodInterceptAttribute() : this(0)
        {
        }

        public MethodInterceptAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }
    }
}
