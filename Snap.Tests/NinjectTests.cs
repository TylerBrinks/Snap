using Ninject;
using NUnit.Framework;
using Snap.Ninject;
using Snap.Tests.Fakes;
using Snap.Tests.Interceptors;

namespace Snap.Tests
{
    [TestFixture]
    public class NinjectTests
    {
        [Test]
        public void Ninject_Container_Supports_Aspects()
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
        }
    }
}
