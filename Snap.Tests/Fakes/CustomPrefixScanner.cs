using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Snap.Interfaces;

namespace Snap.Tests.Fakes
{
    public class CustomPrefixScanner : IScanningConvention
    {
        public List<BindingPair> Scan(Assembly assemblyToScan)
        {
            const string prefixConvention = "TestConvention";

            var types = assemblyToScan.GetTypes();

            var interceptors = types
                .Where(type => type.BaseType == typeof(MethodInterceptor) 
                    && type.Name.StartsWith(prefixConvention))
                .ToDictionary(type => type.Name.Replace("Interceptor", string.Empty));

            var attributes = (from type in types
                              where type.BaseType == typeof(MethodInterceptAttribute) 
                                && type.Name.StartsWith(prefixConvention)
                              select type).ToDictionary(type => type.Name.Replace("Attribute", string.Empty));

            return (from i in interceptors
                    join a in attributes on i.Key equals a.Key
                    select new BindingPair
                    {
                        InterceptorType = i.Value,
                        AttributeType = a.Value
                    }).ToList();
        }
    }
}
