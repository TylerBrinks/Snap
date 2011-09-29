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
using System.Collections.Generic;

namespace Snap
{
    /// <summary>
    /// Provider-agnostic Aspect-Oriented configuration
    /// </summary>
    public class AspectConfiguration : IAspectConfiguration
    {
        private readonly List<string> _namespaces = new List<string>();
        private readonly List<AspectRegistration> _interceptorRegistrations = new List<AspectRegistration>();

        /// <summary>
        /// Registers a method interceptor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public IConfigurationSyntax Bind<T>() where T : IAttributeInterceptor, new()
        {
            _interceptorRegistrations.Add(new AspectRegistration(typeof(T)));
            return new ConfigurationSyntax<T>(this);
        }
        /// <summary>
        /// Includes a type for interception
        /// </summary>
        /// <param name="fullyQualifiedTypeName">Fully qualified type to intercept</param>
        public void IncludeType(string fullyQualifiedTypeName)
        {
            if (!_namespaces.Contains(fullyQualifiedTypeName))
            {
                _namespaces.Add(fullyQualifiedTypeName);
            }
        }
        /// <summary>
        /// Includes a type for interception
        /// </summary>
        /// <param name="type">Type to intercept</param>
        public void IncludeType(Type type)
        {
            IncludeType(type.FullName);
        }
        /// <summary>
        /// Includes a type of type T for interception
        /// </summary>
        public void IncludeType<T>()
        {
            IncludeType(typeof (T).FullName);
        }
        /// <summary>
        /// Includes a namespace for AOP method interception type lookups.
        /// </summary>
        /// <param name="namePrefix">The namespace to include.</param>
        /// <example>My.Type.Name</example>
        public void IncludeNamespace(string namePrefix)
        {
            var name = namePrefix.EndsWith("*") ? namePrefix : namePrefix + "*";
            if (!_namespaces.Contains(name))
            {
                _namespaces.Add(name);
            }
        }
        /// <summary>
        /// Gets the list of configured namespaces.
        /// </summary>
        /// <value>The namespace list.</value>
        public List<string> Namespaces
        {
            get { return _namespaces; }
        }
        /// <summary>
        /// Gets the interceptors.
        /// </summary>
        /// <value>The interceptors.</value>
        public List<AspectRegistration> Interceptors
        {
            get { return _interceptorRegistrations; }
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        public IAspectContainer Container { get; set; }
        /// <summary>
        /// Scans assemblies for type registration.
        /// </summary>
        /// <param name="scanAction">The scanning action.</param>
        /// <returns></returns>
        public void Scan(Action<ITypeScanner> scanAction)
        {
            scanAction(new TypeScanner(this));
        }

        /// <summary>
        /// Give ability to configure selected set of registered aspects in a single step
        /// </summary>
        /// <param name="aspectTypes"></param>
        /// <returns></returns>
        public IAspectBookSyntax Aspects(params Type[] aspectTypes)
        {
            return new AspectBookSyntax(_interceptorRegistrations.Where(aspect => aspectTypes.Contains(aspect.InterceptorType)));
        }

        /// <summary>
        /// Give ability to configure all registered aspects in a single step
        /// </summary>
        public IAspectBookSyntax AllAspects()
        {
            return new AspectBookSyntax(_interceptorRegistrations);
        }

        /// <summary>
        /// Binds an interceptor to an attribute.
        /// </summary>
        /// <typeparam name="T">Interceptor type.</typeparam>
        /// <typeparam name="TAttribute">The type of attribute.</typeparam>
        internal void BindInterceptor<T, TAttribute>()
        {
            BindInterceptor(_interceptorRegistrations.Where(i => i.InterceptorType == typeof(T)).First(), typeof(TAttribute));
        }
        /// <summary>
        /// Adds a binding pair.
        /// </summary>
        /// <param name="aspectRegistration">The interceptor.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        internal void BindInterceptor(AspectRegistration aspectRegistration, Type attributeType)
        {
            aspectRegistration.TargetAttributeType = attributeType;
            if (!_interceptorRegistrations.Contains(aspectRegistration))
            {
                _interceptorRegistrations.Add(aspectRegistration);
            }
        }
        /// <summary>
        /// Adds the binding order for an interceptor.
        /// </summary>
        /// <param name="index">The order index.</param>
        internal void AddBindingOrder(int index)
        {
            _interceptorRegistrations.Last().Order = index;
        }
    }
}