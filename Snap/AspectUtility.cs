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
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Fasterflect;

namespace Snap
{
    /// <summary>
    /// Utility methods
    /// </summary>
    public static class AspectUtility
    {
        /// <summary>
        /// Creates a proxy around an instance with type interceptors.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="instanceToWrap">The instance to wrap.</param>
        /// <param name="interceptors">The interceptors.</param>
        /// <returns>Wrapped instance</returns>
        public static object CreateProxy(Type interfaceType, object instanceToWrap, params IInterceptor[] interceptors)
        {
            if (interfaceType.IsInterface)
                return new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(interfaceType, instanceToWrap, interceptors.ToArray());

            return CreateProxyForConcrete(interfaceType, instanceToWrap, interceptors);
        }

        private static object CreateProxyForConcrete(Type interfaceType, object instanceToWrap, IInterceptor[] interceptors)
        {
            object[] ctorArgs = GetDummyConstructorArgs(interfaceType);

            return new ProxyGenerator().CreateClassProxyWithTarget(interfaceType, instanceToWrap, ctorArgs, interceptors);
        }

        private static object[] GetDummyConstructorArgs(Type type)
        {
            var greediestCtor = type.GetConstructors().OrderBy(x => x.Parameters().Count).LastOrDefault();

            return greediestCtor == null ? new object[0] : new object[greediestCtor.Parameters().Count];
        }

        /// <summary>
        /// Creates a proxy around an instance with pseudo (empty) interceptors.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="instanceToWrap">The instance to wrap.</param>
        /// <returns></returns>
        public static object CreatePseudoProxy(IMasterProxy proxy, Type interfaceType, object instanceToWrap)
        {
            var pseudoList = new IInterceptor[proxy.Configuration.Interceptors.Count];
            pseudoList[0] = proxy;

            for (var i = 1; i < pseudoList.Length; i++)
            {
                pseudoList[i] = new PseudoInterceptor();
            }

            return CreateProxy(interfaceType, instanceToWrap, pseudoList);
        }
        /// <summary>
        /// Determines whether the specified target object has methods decorated for interception.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// 	<c>true</c> if the specified target is decorated; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDecorated(this object target, AspectConfiguration configuration)
        {
            var methods = target.GetType().Methods();

            var isDecorated = methods.Any(m => m.Attributes().Any(a => a is MethodInterceptAttribute));

            return isDecorated;
        }
        /// <summary>
        /// Gets the first type that matches the target namespace.
        /// </summary>
        /// <param name="typeList">The type list.</param>
        /// <param name="namespaces">The namespaces.</param>
        /// <returns></returns>
        public static Type FirstMatch(this Type[] typeList, IList<string> namespaces)
        {
            return typeList.FirstOrDefault(i => namespaces.Any(i.DoesTypeBelongToNamespace));
        }
        /// <summary>
        /// Determines whether the specified value matche.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="test">The test.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is a match; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMatch(this string value, string test)
        {
            return test.Contains("*")
                ? value.StartsWith(test.Replace("*", "")) // Wildcard. Check that the string starts with.
                : value.Equals(test); // Not a wild card. Must be an exact match.
        }

        /// <summary>
        /// Determines the type of DynamicProxy that will be created over the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Type GetTypeToDynamicProxy(this Type type, IList<string> namespaces )
        {
            var allInterfaces = type.GetInterfaces();

            IEnumerable<Type> baseClassInterfaces = type.BaseType != null ? type.BaseType.GetInterfaces() : new Type[0];
            IEnumerable<Type> topLevelInterfaces = allInterfaces.Except(baseClassInterfaces);

            if (topLevelInterfaces.Count() == 0)
            {
                var types = new[] { type };
                return types.FirstMatch(namespaces);
            }

            return allInterfaces.FirstMatch(namespaces);
        }

        private static bool DoesTypeBelongToNamespace(this Type type, string givenNamespace)
        {
            // 1) compare type name for exact match
            // 2) compare namespaces for exact match
            // 3) compare type name to namespace for prefix match assuming given namespace ends with a wildcard
            return type.FullName.Equals(givenNamespace)
                || (type.Namespace != null && type.Namespace.Equals(givenNamespace))
                || (givenNamespace.EndsWith("*") && type.FullName.StartsWith(givenNamespace.Replace("*", "")));
        }
    }
}