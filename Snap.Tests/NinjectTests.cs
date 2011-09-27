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
using Ninject;
using NUnit.Framework;
using Snap.Ninject;
using SnapTests.Fakes;
using Snap.Tests.Interceptors;

namespace Snap.Tests
{
    [TestFixture]
    public class NinjectTests : TestBase
    {
        [Test]
        public void Ninject_Container_Supports_Method_Aspects()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            var badCode = container.Kernel.Get<IBadCode>();
            
            Assert.DoesNotThrow(badCode.GiddyUp);
            Assert.IsTrue(badCode.GetType().Name.Equals("IBadCodeProxy"));
        }

        [Test]
        public void Ninject_Container_Supports_Multiple_Method_Aspects()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            container.Kernel.Bind<IOrderedCode>().To<OrderedCode>();
            var badCode = container.Kernel.Get<IOrderedCode>();
            badCode.RunInOrder();
            Assert.AreEqual("First", OrderedCode.Actions[0]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
        }

        [Test]
        public void Ninject_Container_Ignores_Types_Without_Decoration()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<INotInterceptable>().To<NotInterceptable>();
            var code = container.Kernel.Get<INotInterceptable>();

            Assert.IsFalse(code.GetType().Name.Equals("INotInterceptableProxy"));
        }

        [Test]
        public void Ninject_Container_Allow_Wildcard_Matching()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            var badCode = container.Kernel.Get<IBadCode>();

            Assert.DoesNotThrow(badCode.GiddyUp);
            Assert.IsTrue(badCode.GetType().Name.Equals("IBadCodeProxy"));
        }

        [Test]
        public void Ninject_Container_Allow_Strict_Matching()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes.IBadCode");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            Assert.DoesNotThrow(() => container.Kernel.Get<IBadCode>());
        }

        [Test]
        public void Ninject_Container_Fails_Without_Match()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("Does.Not.Work");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            Assert.Throws<NullReferenceException>(() => container.Kernel.Get<IBadCode>());
        }

        [Test]
        public void Ninject_Supports_Types_Without_Interfaces()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IDependency>().To<DummyDependency>();

            container.Kernel.Bind<TypeWithoutInterface>().To<TypeWithoutInterface>();
            var typeWithoutInterface = container.Kernel.Get<TypeWithoutInterface>();

            Assert.DoesNotThrow(typeWithoutInterface.Foo);
            Assert.IsTrue(typeWithoutInterface.GetType().Name.Equals("TypeWithoutInterfaceProxy"));

            container.Kernel.Bind<TypeWithInterfaceInBaseClass>().To<TypeWithInterfaceInBaseClass>();
            var typeWithInterfaceInBaseClass = container.Kernel.Get<TypeWithInterfaceInBaseClass>();

            Assert.DoesNotThrow(typeWithInterfaceInBaseClass.Foo);
            Assert.IsTrue(typeWithInterfaceInBaseClass.GetType().Name.Equals("TypeWithInterfaceInBaseClassProxy"));
        }

        [Test]
        public void NInject_Container_Does_Not_Support_Resolving_Aspects_From_Container()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();

                // not supported now
                c.AllAspects().KeepInContainer();
            });

            // do not register HandleErrorInterceptor in container
            container.Kernel.Bind<IBadCode>().To<BadCode>();

            // no failure, HandleErrorInterceptor is created via new() and intercepted
            Assert.DoesNotThrow(container.Kernel.Get<IBadCode>().GiddyUp);
        }

        [Test]
        public void Ninject_Supports_Resolving_All_Aspects_From_Container()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.AllAspects().KeepInContainer();
            });

            container.Kernel.Bind<IOrderedCode>().To<OrderedCode>();
            container.Kernel.Bind<FirstInterceptor>().ToConstant(new FirstInterceptor("first_kept_in_container"));
            container.Kernel.Bind<SecondInterceptor>().ToConstant(new SecondInterceptor("second_kept_in_container"));

            var orderedCode = container.Kernel.Get<IOrderedCode>();
            orderedCode.RunInOrder();

            CollectionAssert.AreEquivalent(
                OrderedCode.Actions,
                new[] { "first_kept_in_container", "second_kept_in_container" });
        }

        [Test]
        public void Ninject_Supports_Resolving_Only_Selected_Aspects_From_Container()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.Aspects(typeof(FirstInterceptor)).KeepInContainer();
            });

            container.Kernel.Bind<IOrderedCode>().To<OrderedCode>();
            container.Kernel.Bind<FirstInterceptor>().ToConstant(new FirstInterceptor("first_kept_in_container"));
            container.Kernel.Bind<SecondInterceptor>().ToConstant(new SecondInterceptor("second_kept_in_container"));

            var orderedCode = container.Kernel.Get<IOrderedCode>();
            orderedCode.RunInOrder();

            // first interceptor is resolved from container, while second one is via new() 
            CollectionAssert.AreEquivalent(
                OrderedCode.Actions,
                new[] { "first_kept_in_container", "Second" });
        }
    }
}
