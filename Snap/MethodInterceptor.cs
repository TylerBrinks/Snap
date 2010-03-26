using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Interceptor;
using Fasterflect;

namespace Snap
{
    /// <summary>
    /// Intercepts method calls for configured types
    /// </summary>
    public abstract class MethodInterceptor : IAttributeInterceptor
    {
        private static readonly Dictionary<string, Attribute> SignatureCache = new Dictionary<string, Attribute>();

        /// <summary>
        /// Gets or sets the target attribute.
        /// </summary>
        /// <value>The target attribute.</value>
        public Type TargetAttribute { get; set; }
        /// <summary>
        /// Gets or sets the invocation order.
        /// </summary>
        /// <value>The order.</value>
        public int Order{ get; set; }

        /// <summary>
        /// Intercepts the method.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="method">The method.</param>
        /// <param name="attribute">The attribute.</param>
        public abstract void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute);
        /// <summary>
        /// Called immediately before interceptor invocation.
        /// </summary>
        public virtual void BeforeInvocation()
        {
        }
        /// <summary>
        /// Called immediately after interceptor invocation.
        /// </summary>
        public virtual void AfterInvocation()
        {
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
                .GetType().Method(invocation.MethodInvocationTarget.Name, parameters.Select(p => p.ParameterType).ToArray());
                //.GetMethod(invocation.MethodInvocationTarget.Name, parameters.Select(p => p.ParameterType).ToArray());

            // Searches for decoration on the method for a given interceptor
            var attribute = GetAttribute(method);

            if (attribute != null)
            {
                // Intercept the method
                InterceptMethod(invocation, method, attribute);
            }
            else
            {
                // No attribute - proceed to the method or the next interceptor.
                invocation.Proceed();
            }
        }
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
                             where attr.GetType().Equals(TargetAttribute)
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
            var parameters = from m in method.Parameters()
                             select m.ParameterType.ToString();

            return string.Format("{0}+{1}", TargetAttribute.FullName, MethodSignatureFormatter.Create(method, parameters.ToArray()));
        }
    }
}
