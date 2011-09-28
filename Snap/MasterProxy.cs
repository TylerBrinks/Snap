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
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.Practices.ServiceLocation;

namespace Snap {
    public class MasterProxy: IMasterProxy {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public AspectConfiguration  Configuration { get; set;}

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IServiceLocator      Container { get; set; }

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
                                    select ResolveHowToCreateInterceptor(interceptor).Create(interceptor)).ToList();
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

        private IInterceptorCreationStrategy ResolveHowToCreateInterceptor(AspectRegistration aspectRegistration)
        {
            IInterceptorCreationStrategy strategy;
            if(Container == null)
            {
                // when container is not specified, always use InstantiateInterceptorDirectlyCreationStrategy 
                // without considering AspectRegistration.KeptInContainer setting 
                strategy = new InstantiateInterceptorDirectlyCreationStrategy();
            }
            else if (aspectRegistration.KeptInContainer)
            {
                strategy = new ResolveInterceptorFromContainerCreationStrategy(Container);
            }
            else
            {
                strategy = new InstantiateInterceptorDirectlyCreationStrategy();
            }
            return strategy;
        }
    }
}