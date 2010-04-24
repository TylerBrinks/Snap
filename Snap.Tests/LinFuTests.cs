using System;
using LinFu.IoC;
using NUnit.Framework;
using Snap.LinFu;
using SnapTests.Fakes;
using Snap.Tests.Interceptors;

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
    }
}
