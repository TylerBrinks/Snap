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
    /// Defines aspect-related settings which are configured by user
    /// Interceptor
    /// </summary>
    public class AspectRegistration
    {
        public AspectRegistration(Type interceptorType) : this(interceptorType, null, 0)
        {}

        public AspectRegistration(Type interceptorType, Type targetAttributeType, int order)
        {
            InterceptorType = interceptorType;
            TargetAttributeType = targetAttributeType;
            Order = order;

            // by default interceptor is not kept in container, and created via constructor rather than resolved from container
            KeptInContainer = false;
        }

        /// <summary>
        /// Gets the type of the interceptor.
        /// </summary>
        public Type InterceptorType { get; private set; }

        /// <summary>
        /// Gets or sets the type of attribute, which applies the aspect to a method.
        /// </summary>
        public Type TargetAttributeType { get; set; }


        /// <summary>
        /// Gets or sets the order of aspect execution if several aspects are applied to the same method.
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether aspect instance should be resolved from container when method is intercepted.
        /// </summary>
        public bool KeptInContainer { get; set; }
    }
}