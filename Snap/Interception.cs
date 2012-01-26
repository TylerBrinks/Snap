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
using System.Reflection;
using Castle.DynamicProxy;
using Fasterflect;
using System.Text.RegularExpressions;

namespace Snap
{
    /// <summary>
    /// Represents detailed info about an interception.
    /// Interception notion is a binding of the interceptor type being applied to the specific method, which is decorated with the specific attribute.
    /// </summary>
    public class Interception
    {
        private static readonly Dictionary<string, Attribute> SignatureCache = new Dictionary<string, Attribute>();

        private Interception(
            IInvocation invocation, 
            MethodBase targetMethod, 
            Type interceptorType, 
            Attribute attributeInstance)
        {
            Invocation = invocation;
            TargetMethod = targetMethod;
            InterceptorType = interceptorType;
            AttributeInstance = attributeInstance;
        }

        /// <summary>
        /// Gets the method invocation.
        /// </summary>
        public IInvocation  Invocation { get; private set; }

        /// <summary>
        /// Gets the target method of the invocation.
        /// </summary>
        public MethodBase   TargetMethod { get; private set; }

        /// <summary>
        /// Gets the type of the interceptor.
        /// </summary>
        public Type         InterceptorType { get; private set; }

        /// <summary>
        /// Gets the instance of the attribute, which applies interceptor to a method.
        /// </summary>
        public Attribute    AttributeInstance { get; private set; }
        
        /// <summary>
        /// Gets detailed info about current interception
        /// </summary>
        /// <param name="attributeType">Attribute which applies interceptor to a method</param>
        /// <param name="invocation">Method invocation info</param>
        /// <returns></returns>
        public static Interception GetCurrent(Type attributeType, IInvocation invocation)
        {
            var targetMethod = GetTargetMethod(invocation);
            var attributeInstance = GetAttribute(invocation.TargetType, targetMethod, attributeType);
            return new Interception(invocation, targetMethod, invocation.TargetType, attributeInstance);
        }

        /// <summary>
        /// Tell whether current invoked method is decorated with specified attribute.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="attributeType">Type of the attribute, which applies interceptor to a method.</param>
        /// <returns></returns>
        public static bool DoesTargetMethodHaveAttribute(IInvocation invocation, Type attributeType)
        {
            var targetMethod = GetTargetMethod(invocation);
            var attributeInstance = GetAttribute(invocation.TargetType, targetMethod, attributeType);
            return attributeInstance != null;
        }
        
        /// <summary>
        /// Gets the method, which is represented by the invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns></returns>
        private static MethodBase GetTargetMethod(IInvocation invocation)
        {
            // Gets the method's parameters (argument types)
            var parameters = invocation.MethodInvocationTarget.GetParameters();

            // Gets the concrete type's method by name and argument type
            var method = invocation
                .InvocationTarget
                .GetType().Method(invocation.MethodInvocationTarget.Name, parameters.Select(p => p.ParameterType).ToArray());

            // If the method is generic, I need to check each parameter to find the correct method
            if (method == null)
            {
                foreach (var m in invocation.InvocationTarget.GetType().GetMethods().Where(x => x.Name == invocation.MethodInvocationTarget.Name))
                {
                    var exists = true;

                    foreach (var p in parameters.Where(p => !m.Parameters().Any(x => x.Name == p.Name)))
                    {
                        exists = false;
                    }

                    if (exists)
                    {
                        method = m;
                    }
                }
            }

            return method;
        }

        /// <summary>
        /// Gets the instance of the attribute with given type, which decorates given method on given type.
        /// </summary>
        /// <param name="targetType">Type of the target type.</param>
        /// <param name="method">The target method.</param>
        /// <param name="attributeType">Type of the attribute, which applies interceptor to a method.</param>
        /// <returns></returns>
        private static Attribute GetAttribute(Type targetType, MethodBase method, Type attributeType)
        {
            var key = GetMethodSignature(method, attributeType);
            if (SignatureCache.ContainsKey(key))
            {
                return SignatureCache[key];
            }

            var classAttributes = (from attr in targetType.GetCustomAttributes(!targetType.IsInterface)
                                   where attr.GetType().Equals(attributeType)
                                   select attr).ToList();

            if (classAttributes.Any())
            {
                bool match = true;
                var attribute = (ClassInterceptAttribute)classAttributes.First();
                if (!String.IsNullOrEmpty(attribute.IncludePattern))
                {
                    match = Regex.IsMatch(method.Name, attribute.IncludePattern, RegexOptions.Singleline);
                }

                if (match && !String.IsNullOrEmpty(attribute.ExcludePattern))
                {
                    match = !Regex.IsMatch(method.Name, attribute.ExcludePattern, RegexOptions.Singleline);
                }

                if (match)
                {
                    SignatureCache.Add(key, attribute);
                    return attribute;
                }
            }

            var attributes = (from attr in method.GetCustomAttributes(!targetType.IsInterface)
                             where attr.GetType().Equals(attributeType)
                             select attr).ToList();

            if (attributes.Any())
            {
                var attribute = (Attribute)attributes.First();
                SignatureCache.Add(key, attribute);
                return attribute;
            }
            else
            {
                SignatureCache.Add(key, null);
            }

            return null;
        }

        /// <summary>
        /// Gets the method signature in string format.
        /// </summary>
        /// <param name="method">The method signature.</param>
        /// <param name="attributeType">Type of attribute, which applies aspect to a method</param>
        /// <returns></returns>
        private static string GetMethodSignature(MethodBase method, Type attributeType)
        {
            var parameters = from m in method.Parameters()
                             select m.ParameterType.ToString();

            return string.Format("{0}+{1}", attributeType.FullName, MethodSignatureFormatter.Create(method, parameters.ToArray()));
        }
    }
}