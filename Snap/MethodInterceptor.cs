/*
Snap v1.0

Copyright (c) 2010 Tyler Brinks

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Fasterflect;

namespace Snap {
    /// <summary>
    /// Intercepts method calls for configured types
    /// </summary>
    public abstract class MethodInterceptor: IAttributeInterceptor {
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
        public int Order { get; set; }

        /// <summary>
        /// Intercepts the method.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="method">The method.</param>
        /// <param name="attribute">The attribute.</param>
        public abstract void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute);

        /// <summary>
        /// Determines whether a type shoulds be intercepted by this interceptor.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns></returns>
        public bool ShouldIntercept(IInvocation invocation) {
            // Gets the method's parameters (argument types).
            var parameters = invocation.MethodInvocationTarget.GetParameters();

            // Gets the concrete type's method by name and argument type.
            var method = invocation
                .InvocationTarget
                .GetType().Method(invocation.MethodInvocationTarget.Name, parameters.Select(p => p.ParameterType).ToArray());

            // Searches for decoration on the method for a given interceptor.
            return GetAttribute(method) != null;
        }
        /// <summary>
        /// Called immediately before interceptor invocation.
        /// </summary>
        public virtual void BeforeInvocation() {
        }
        /// <summary>
        /// Called immediately after interceptor invocation.
        /// </summary>
        public virtual void AfterInvocation() {
        }
        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation) {
            // Gets the method's parameters (argument types)
            var parameters = invocation.MethodInvocationTarget.GetParameters();

            // Gets the concrete type's method by name and argument type
            var method = invocation
                .InvocationTarget
                .GetType().Method(invocation.MethodInvocationTarget.Name, parameters.Select(p => p.ParameterType).ToArray());

            // Searches for decoration on the method for a given interceptor
            var attribute = GetAttribute(method);

            // Intercept the method.  It's safe to avoid checking the attribute for null
            // since the ShouldIntercept method always preceeds this method call.
            InterceptMethod(invocation, method, attribute);
        }
        /// <summary>
        /// Gets a methods attribute.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        private Attribute GetAttribute(MethodBase method) {
            var key = GetMethodSignature(method);

            if(SignatureCache.ContainsKey(key)) {
                return SignatureCache[key];
            }

            var attributes = from attr in method.GetCustomAttributes(false)
                             where attr.GetType().Equals(TargetAttribute)
                             select attr;

            if(attributes.Any()) {
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
        private string GetMethodSignature(MethodBase method) {
            var parameters = from m in method.Parameters()
                             select m.ParameterType.ToString();

            return string.Format("{0}+{1}", TargetAttribute.FullName, MethodSignatureFormatter.Create(method, parameters.ToArray()));
        }
    }
}