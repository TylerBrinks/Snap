using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Snap
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ClassInterceptAttribute : Attribute, IInterceptAttribute
    {
        public ClassInterceptAttribute()
            : this(0, null, null, MulticastOptions.None)
        {

        }

        public ClassInterceptAttribute(int order, string include, string exclude, MulticastOptions multicastOptions)
        {
            Order = order;
            IncludePattern = include;
            ExcludePattern = exclude;
            MulticastOptions = multicastOptions;
        }
        
        public int Order { get; set; }

        public string IncludePattern { get; set; }

        public string ExcludePattern { get; set; }

        public MulticastOptions MulticastOptions { get; set; }

        /// <summary>
        /// The type of methods to bind to. Due to dynamic proxy limitations, certain field types are not specified,
        /// i.e. static.
        /// </summary>
    }

    [Flags]
    public enum MulticastOptions
    {
        /// <summary>
        /// Public and Private methods
        /// </summary>
        None = 0,
        Public = 1,
        Private = 2,
        Protected = 4
    }
}
