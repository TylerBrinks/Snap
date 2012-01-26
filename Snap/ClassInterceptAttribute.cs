using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snap
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ClassInterceptAttribute : Attribute, IInterceptAttribute
    {
        public ClassInterceptAttribute()
            : this(0, null, null)
        {

        }

        public ClassInterceptAttribute(int order, string include, string exclude)
        {
            Order = order;
            IncludePattern = include;
            ExcludePattern = exclude;
        }
        
        public int Order { get; set; }

        public string IncludePattern { get; set; }

        public string ExcludePattern { get; set; }
    }
}
