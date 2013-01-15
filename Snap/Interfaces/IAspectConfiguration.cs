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
    /// Defines syntax for configuring aspects
    /// </summary>
    public interface IAspectConfiguration : IHideBaseTypes
    {
        /// <summary>
        /// Creates an interceptor binding.
        /// </summary>
        /// <typeparam name="T">Type of interceptor</typeparam>
        /// <returns>IConfigurationSyntax instance</returns>
        IConfigurationSyntax Bind<T>() where T : IAttributeInterceptor, new();

        /// <summary>
        /// Includes a type for interception
        /// </summary>
        /// <param name="fullyQualifiedTypeName">Fully qualified type to intercept</param>
        void IncludeType(string fullyQualifiedTypeName);

        /// <summary>
        /// Includes a type for interception
        /// </summary>
        /// <param name="type">Type to intercept</param>
        void IncludeType(Type type);

        /// <summary>
        /// Includes a type of type T for interception
        /// </summary>
        void IncludeType<T>();

        /// <summary>
        /// Includes a namespace for type interception.  All types starting with this namespace will be matched.
        /// </summary>
        /// <param name="name">The namespace.</param>
        void IncludeNamespace(string name);

        /// <summary>
        /// Includes a namespace of a given type for AOP method interception type lookups. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void IncludeNamespaceOf<T>();

        /// <summary>
        /// Includes a namespace of a given type for AOP method interception type lookups. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeNestedNamespaces">
        /// When true, nested namespaces are included too, otherwise only given type's namespace is included
        /// </param>
        void IncludeNamespaceOf<T>(bool includeNestedNamespaces);

        /// <summary>
        /// Scans assemblies for type registration.
        /// </summary>
        /// <param name="scanAction">The scanning action.</param>
        void Scan(Action<ITypeScanner> scanAction);

        /// <summary>
        /// Give ability to configure selected set of registered aspects in a single step
        /// </summary>
        /// <param name="aspectTypes"></param>
        /// <returns></returns>
        IAspectBookSyntax Aspects(params Type[] aspectTypes);

        /// <summary>
        /// Give ability to configure all registered aspects in a single step
        /// </summary>
        /// <returns></returns>
        IAspectBookSyntax AllAspects();
    }
}
