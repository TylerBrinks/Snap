using System;
using System.Reflection;
using Snap.Interfaces;

namespace Snap
{
    public class TypeScanner : ITypeScanner, ITypeScanningStrategy
    {
        private readonly AspectConfiguration _configuration;
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
            With(new DefaultConventionScanner());
        }

        /// <summary>
        /// Scans assemblies with a custom convention.
        /// </summary>
        /// <param name="scanner">The custom assembly scanner.</param>
        public void With(IScanningConvention scanner)
        {
            var pairs = scanner.Scan(_assembly);

            pairs.ForEach(p =>
            {
                var interceptor = (IAttributeInterceptor)Activator.CreateInstance(p.InterceptorType);
                _configuration.BindInterceptor(interceptor, p.AttributeType);
            });
        }
    }
}
