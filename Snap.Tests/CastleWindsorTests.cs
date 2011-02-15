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
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Snap.CastleWindsor;
using SnapTests.Fakes;
using Snap.Tests.Interceptors;

namespace Snap.Tests
{
    [TestFixture]
    public class CastleWindsorTests : TestBase
    {
        [Test]
        public void CastleWindsor_Container_Supports_Method_Aspects()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
                                {
                                    c.IncludeNamespace("SnapTests*");
                                    c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                                });

            container.Register(Component.For(typeof (IBadCode)).ImplementedBy(typeof (BadCode)).Named("BadCode"));
            var badCode = container.Resolve<IBadCode>();
            Assert.DoesNotThrow(badCode.GiddyUp);
        }

        [Test]
        public void CastleWindsor_Container_Supports_Multiple_Method_Aspects()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            container.Register(
                Component.For(typeof (IOrderedCode)).ImplementedBy(typeof (OrderedCode)).Named("OrderedCode"));
            var orderedCode = container.Resolve<IOrderedCode>();
            orderedCode.RunInOrder();
            Assert.AreEqual("First", OrderedCode.Actions[0]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
        }
    }
}
