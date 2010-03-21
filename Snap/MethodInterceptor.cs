using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// Intercepts method calls for configured types
    /// </summary>
    public abstract class MethodInterceptor : IInterceptor
    {
        private static readonly Dictionary<string, Attribute> SignatureCache = new Dictionary<string, Attribute>();
        private readonly Type _attributeType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInterceptor"/> class.
        /// </summary>
        /// <param name="attributeType">Type of the attribute.</param>
        protected MethodInterceptor(Type attributeType)
        {
            _attributeType = attributeType;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            // Gets the method's parameters (argument types)
            var parameters = invocation.MethodInvocationTarget.GetParameters();

            // Gets the concrete type's method by name and argument type
            var method = invocation
                .InvocationTarget
                .GetType()
                .GetMethod(invocation.MethodInvocationTarget.Name, parameters.Select(p => p.ParameterType).ToArray());

            // Searches for decoration on the method for a given interceptor
            var attribute = GetAttribute(method);

            if (attribute != null)
            {
                // Intercept the method
                InterceptMethod(invocation, method, attribute);
            }
            else
            {
                // No attribute - continue to process the method (or next interceptor).
                invocation.Proceed();
            }
        }
        /// <summary>
        /// Intercepts the method.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="method">The method.</param>
        /// <param name="attribute">The attribute.</param>
        public abstract void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute);
        /// <summary>
        /// Gets a methods attribute.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        private Attribute GetAttribute(MethodBase method)
        {
            var key = GetMethodSignature(method);

            if (SignatureCache.ContainsKey(key))
            {
                return SignatureCache[key];
            }

            var attributes = from attr in method.GetCustomAttributes(false)
                             where attr.GetType().Equals(_attributeType)
                             select attr;

            if (attributes.Any())
            {
                var attribute = (Attribute)attributes.First();
                SignatureCache.Add(key, attribute);
                return attribute;
            }

            return null;
        }
        /// <summary>
        /// Gets the method signature in string format.
        /// </summary>
        /// <param name="method">The method signature.</param>
        /// <returns></returns>
        private string GetMethodSignature(MethodBase method)
        {
            var parameters = from m in method.GetParameters()
                             select m.ParameterType.ToString();

            return string.Format("{0}+{1}",_attributeType.FullName, MethodSignatureFormatter.Create(method, parameters.ToArray()));
        }
    }
}
