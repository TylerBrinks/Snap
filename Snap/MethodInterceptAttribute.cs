using System;

namespace Snap
{
    /// <summary>
    /// Used to decorate methods for interception.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public class MethodInterceptAttribute : Attribute, IInterceptAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInterceptAttribute"/> class.
        /// </summary>
        public MethodInterceptAttribute() : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInterceptAttribute"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        public MethodInterceptAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }
    }
}
