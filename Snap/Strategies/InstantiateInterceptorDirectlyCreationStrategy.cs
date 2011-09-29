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

namespace Snap
{
    /// <summary>
    /// Defines strategy to create interceptor instances
    /// Creates interceptor directly by calling <see cref="Activator.CreateInstance(System.Type)">>Activator.CreateInstance</see> method.
    /// </summary>
    public class InstantiateInterceptorDirectlyCreationStrategy : IInterceptorCreationStrategy
    {
        /// <summary>
        /// Creates instance of <paramref name="aspectRegistration">interceptor</paramref> for given interceptor registration
        /// </summary>
        /// <param name="aspectRegistration">Registration of interceptor, is built on aspect configuration build step</param>
        /// <returns></returns>
        public IAttributeInterceptor Create(AspectRegistration aspectRegistration)
        {
            // Assume interceptor type implements IAttributeInterceptor contract and has parameterless constructor. 
            // This rule is enforced on aspect configuration build step
            var interceptor = (IAttributeInterceptor)Activator.CreateInstance(aspectRegistration.InterceptorType);

            // reassign target attribute and order properties from interception registration to actual interceptor instance
            interceptor.TargetAttribute = aspectRegistration.TargetAttributeType;
            interceptor.Order = aspectRegistration.Order;

            return interceptor;
        }
    }
}