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
using System.Reflection;
using Snap.Interfaces;

namespace Snap
{
    internal class DefaultConventionScanner : IScanningConvention
    {
        /// <summary>
        /// Scans for interceptor/attribute pairs based on a custom convention.
        /// </summary>
        /// <param name="assemblyToScan">The assembly to scan.</param>
        /// <returns>
        /// Binding pairs with matching prefix type names.
        /// </returns>
        public List<BindingPair> Scan(Assembly assemblyToScan)
        {
            const string interceptorConvention = "Interceptor";
            const string attributeConvention = "Attribute";

            var types = assemblyToScan.GetTypes();

            var interceptors = types
                .Where(type => type.BaseType == typeof(MethodInterceptor))
                .ToDictionary(type => type.Name.Replace(interceptorConvention, string.Empty));

            var attributes = (from type in types
                              where type.GetInterface(typeof(IInterceptAttribute).Name) != null
                              let typeName = type.Name.Replace(attributeConvention, string.Empty)
                              where interceptors.Any(i => i.Key == typeName)
                              select type).ToDictionary(type => type.Name.Replace(attributeConvention, string.Empty));

           return (from i in interceptors
                        join a in attributes on i.Key equals a.Key
                        select new BindingPair
                                   {
                                       InterceptorType = i.Value, 
                                       AttributeType = a.Value
                                   }).ToList();
        }
    }
}
