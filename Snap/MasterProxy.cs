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
using System.Linq;
using Castle.DynamicProxy;

namespace Snap {
    public class MasterProxy: IMasterProxy {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public AspectConfiguration Configuration {
            get;
            set;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            var interceptors = Configuration.Interceptors;
            var orderedInterceptors = SortOrderFactory.GetSortOrderStrategy(invocation, interceptors).Sort();
            var validInterceptors = (from interceptor in orderedInterceptors
                                    where Interception.DoesTargetMethodHaveAttribute(invocation, interceptor.TargetAttributeType)
                                    select ResolveInterceptorInstance(interceptor)).ToList();
            var falseInvocations = orderedInterceptors.Count() - validInterceptors.Count();

            for(var i = 0; i < falseInvocations; i++) {
                // Not all interceptors run for each type, but all interceptors are interrogated.
                // If there are 5 interceptors, but only 1 attribute, this handles the other 4
                // necessary invocations.
                invocation.Proceed();
            }

            foreach(var interceptor in validInterceptors) {
                interceptor.BeforeInvocation();
                interceptor.Intercept(invocation);
                interceptor.AfterInvocation();
            }
        }

        private static IAttributeInterceptor ResolveInterceptorInstance(InterceptorRegistration interceptorRegistration)
        {
            // Assume interceptor type implements IAttributeInterceptor contract. 
            // This rule is enforced on aspect configuration build step
            // Create new instance by means of constructor. Switch using service locator later.
            var interceptor = (IAttributeInterceptor) Activator.CreateInstance(interceptorRegistration.InterceptorType);
            
            // reassign target attribute and order properties from interception registration to actual interceptor instance
            interceptor.TargetAttribute = interceptorRegistration.TargetAttributeType;
            interceptor.Order = interceptorRegistration.Order;

            return interceptor;
        }
    }
}