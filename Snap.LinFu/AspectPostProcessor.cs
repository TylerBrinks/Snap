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
using Castle.DynamicProxy;
using LinFu.IoC;
using LinFu.IoC.Interfaces;

namespace Snap.LinFu
{
    public class AspectPostProcessor : IPostProcessor
    {
        public void PostProcess(IServiceRequestResult result)
        {
            var instance = result.ActualResult;
            var instanceTypeName = instance.GetType().FullName;
           
            // Ignore any LinFu factories or Snap-specific instances.
            if (instanceTypeName.Contains("LinFu.") || instanceTypeName == "Snap.AspectConfiguration"
                || instanceTypeName == "Snap.IMasterProxy" || instanceTypeName == "Snap.MasterProxy")
            {
                return;
            }

            var proxy = result.Container.GetService<IMasterProxy>();

            if (!instance.IsDecorated(proxy.Configuration))
            {
                return;
            }

            var pseudoList = new IInterceptor[proxy.Configuration.Interceptors.Count];
            pseudoList[0] = proxy;

            for (var i = 1; i < pseudoList.Length; i++)
            {
                pseudoList[i] = new PseudoInterceptor();
            }

            var interfaceTypes = instance.GetType().GetInterfaces();
            var targetInterface =
                interfaceTypes.FirstMatch(proxy.Configuration.Namespaces);

            result.ActualResult = new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(targetInterface, instance, pseudoList);
        }
    }
}
