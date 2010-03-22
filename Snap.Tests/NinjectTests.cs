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
            var ct = new NinjectAspectContainer();

            SnapConfiguration.For(ct, c =>
                                          {
                                              c.IncludeNamespace("Snap.Tests");
                                              c.RegisterInterceptor<HandleErrorInterceptor>();
                                          });

            ct.Kernel.Bind<IBadCode>().To<BadCode>();
            var badCode = ct.Kernel.Get<IBadCode>();
            Assert.DoesNotThrow(badCode.GiddyUp);
        }
    }
}
