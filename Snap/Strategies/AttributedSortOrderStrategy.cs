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
using System.Linq;

namespace Snap
{
    /// <summary>
    /// Sorts interceptors by attribute Order values.
    /// </summary>
    public class AttributedSortOrderStrategy : ISortOrderStrategy
    {
        private readonly List<AspectRegistration> _interceptors;
        private readonly List<IInterceptAttribute> _attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributedSortOrderStrategy"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="interceptors">The interceptors.</param>
        public AttributedSortOrderStrategy(List<IInterceptAttribute> attributes, List<AspectRegistration> interceptors)
        {
            _interceptors = interceptors;
            _attributes = attributes;
        }

        /// <summary>
        /// Sorts interceptors registrations by attribute order, then by name.
        /// </summary>
        /// <returns>Sorted interceptors</returns>
        public List<AspectRegistration> Sort()
        {
            return _attributes
                .OrderBy(a => a.Order)
                .ThenBy(a => a.GetType().Name).Select(attribute => attribute.GetType())
                .Select(type => _interceptors.First(i => i.TargetAttributeType == type)).ToList();
        }
    }
}
