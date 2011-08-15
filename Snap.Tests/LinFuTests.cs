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
using NUnit.Framework;
using LinFu.IoC;
using Snap.LinFu;
using Snap.Tests.Interceptors;
using SnapTests.Fakes;

namespace Snap.Tests
{
    [TestFixture]
    public class LinFuTests : TestBase
    {
        [Test]
        public void LinFu_Container_Supports_Method_Aspects()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            var x = container.GetService<IBadCode>();
            Assert.DoesNotThrow(x.GiddyUp);
        }

        [Test]
        public void LinFu_Container_Supports_Multiple_Method_Aspects()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            var code = container.GetService<IOrderedCode>();
            code.RunInOrder();

            Assert.AreEqual("First", OrderedCode.Actions[0]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
        }

        [Test]
        public void LinFu_Container_Ignores_Types_Without_Decoration()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
            });

            var code = container.GetService<INotInterceptable>();
          
            Assert.IsFalse(code.GetType().Name.Equals("INotInterceptableProxy"));
        }

        [Test]
        public void LinFu_Container_Allow_Wildcard_Matching()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            Assert.DoesNotThrow(() => container.GetService<IBadCode>());
        }

        [Test]
        public void LinFu_Container_Allow_Strict_Matching()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            Assert.DoesNotThrow(() => container.GetService<IBadCode>());

        }

        [Test]
        public void LinFu_Container_Fails_Without_Match()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("Does.Not.Work");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            Assert.Throws<NullReferenceException>(() => container.GetService<IBadCode>());
        }

        [Test]
        public void LinFu_Supports_Types_Without_Interfaces()
        {
            var container = new ServiceContainer();

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.AddService(typeof(IDependency), typeof(DummyDependency));
            container.AddService(typeof(TypeWithoutInterface));
            container.AddService(typeof(TypeWithInterfaceInBaseClass));

            var typeWithoutInterface = container.GetService<TypeWithoutInterface>();
            Assert.DoesNotThrow(typeWithoutInterface.Foo);
            Assert.IsTrue(typeWithoutInterface.GetType().Name.Equals("TypeWithoutInterfaceProxy"));

            var typeWithInterfaceInBaseClass = container.GetService<TypeWithInterfaceInBaseClass>();
            Assert.DoesNotThrow(typeWithInterfaceInBaseClass.Foo);
            Assert.IsTrue(typeWithInterfaceInBaseClass.GetType().Name.Equals("TypeWithInterfaceInBaseClassProxy"));
        }
    }
}
