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
    }
}
