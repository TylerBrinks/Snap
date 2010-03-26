using Ninject;
using NUnit.Framework;
using Snap.Ninject;
using Snap.Tests.Fakes;
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
                c.IncludeNamespace("Snap.Tests");
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
                c.IncludeNamespace("Snap.Tests");
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
                c.IncludeNamespace("Snap.Tests");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<INotInterceptable>().To<NotInterceptable>();
            var code = container.Kernel.Get<INotInterceptable>();

            Assert.IsFalse(code.GetType().Name.Equals("INotInterceptableProxy"));
        }
    }
}
