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
using System.Reflection;
using Castle.DynamicProxy;

namespace Snap {
    /// <summary>
    /// Intercepts method calls for configured types
    /// </summary>
    public abstract class MethodInterceptor: IAttributeInterceptor {
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
        /// <remarks>
        /// Actually we do not need this method here anymore. 
        /// The responsibility of checking which interceptors should be run is moved to <see cref="MasterProxy">MasterProxy</see> class.
        /// The method is left to avoid breaking existing <see cref="IAttributeInterceptor">IAttributeInterceptor</see> contract.
        /// </remarks>
        /// <returns></returns>
        public bool ShouldIntercept(IInvocation invocation)
        {
            // just delegate it to Interception class
            return Interception.DoesTargetMethodHaveAttribute(invocation, TargetAttribute);
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

            var interception = Interception.GetCurrent(TargetAttribute, invocation);
            InterceptMethod(
                interception.Invocation,
                interception.TargetMethod,
                interception.AttributeInstance);
        }
    }
}