using System;
using LinFu.IoC;
using NUnit.Framework;
using Snap.LinFu;
using Snap.Tests.Fakes;
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
                c.IncludeNamespace("Snap");
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
                c.IncludeNamespace("Snap.Tests");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            var code = container.GetService<IOrderedCode>();
            code.RunInOrder();

            Assert.AreEqual("First", OrderedCode.Actions[0]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
        }

        [Test]
        public void Structuremap_Container_Ignores_Types_Without_Decoration()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("Snap.Tests");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
            });

            var code = container.GetService<INotInterceptable>();
          
            Assert.IsFalse(code.GetType().Name.Equals("INotInterceptableProxy"));
        }
    }
}
