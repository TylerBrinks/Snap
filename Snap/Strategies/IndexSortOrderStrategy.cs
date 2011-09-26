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
using System.Collections.Generic;

namespace Snap
{
    /// <summary>
    /// Sorts interceptors by their configured order index.
    /// </summary>
    public class IndexSortOrderStrategy : ISortOrderStrategy
    {
        private readonly List<InterceptorRegistration> _interceptors;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexSortOrderStrategy"/> class.
        /// </summary>
        /// <param name="interceptors">The interceptors.</param>
        public IndexSortOrderStrategy(List<InterceptorRegistration> interceptors)
        {
            _interceptors = interceptors;
        }

        /// <summary>
        /// Sorts interceptors by their configured order index, then by attribute name.
        /// </summary>
        /// <returns></returns>
        public List<InterceptorRegistration> Sort()
        {
            return _interceptors.OrderBy(i => i.Order).ThenBy(i => i.TargetAttributeType.Name).ToList();
        }
    }
}
