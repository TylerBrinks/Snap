using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Snap
{
    public class TypeScanner : ITypeScanner, ITypeScanningStrategy
    {
        private AspectConfiguration _configuration;
        private Assembly _assembly;

        public TypeScanner(AspectConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ITypeScanningStrategy ThisAssembly()
        {
            _assembly = System.Reflection.Assembly.GetCallingAssembly();

            return this;
        }

        public ITypeScanningStrategy Assembly(Assembly assembly)
        {
            _assembly = assembly;

            return this;
        }

        public void WithDefaults()
        {
            const string interceptorConvention = "Interceptor";
            const string attributeConvention = "Attribute";

            var types = _assembly.GetTypes();

            var interceptors = types
                .Where(type => type.BaseType == typeof(MethodInterceptor))
                .ToDictionary(type => type.Name.Replace(interceptorConvention, string.Empty));

            var attributes = (from type in types
                              where type.BaseType == typeof(MethodInterceptAttribute)
                              let typeName = type.Name.Replace(attributeConvention, string.Empty)
                              where interceptors.Any(i => i.Key == typeName)
                              select type).ToDictionary(type => type.Name.Replace(attributeConvention, string.Empty));

            var pairs = from i in interceptors
                        join a in attributes on i.Key equals a.Key
                        select new { InterceptorType = i.Value, AttributeType = a.Value };

            pairs.ToList().ForEach(p =>
            {
                var interceptor = (IAttributeInterceptor)Activator.CreateInstance(p.InterceptorType);
                _configuration.BindInterceptor(interceptor, p.AttributeType);
            });
        }

        //public void With()
        //{
        //    //scan
        //}
    }
}
