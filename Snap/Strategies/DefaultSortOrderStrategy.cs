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
using System.Collections.Generic;

namespace Snap
{
    /// <summary>
    /// Sorts interceptors in their default configured order.
    /// </summary>
    public class DefaultSortOrderStrategy : ISortOrderStrategy
    {
        private readonly List<AspectRegistration> _interceptors;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSortOrderStrategy"/> class.
        /// </summary>
        /// <param name="interceptors">The interceptors.</param>
        public DefaultSortOrderStrategy(List<AspectRegistration> interceptors)
        {
            _interceptors = interceptors;
        }

        /// <summary>
        /// Sorts interceptor registrations in the configured order.
        /// </summary>
        /// <returns>Sorted interceptors</returns>
        public List<AspectRegistration> Sort()
        {
            return _interceptors;
        }
    }
}
