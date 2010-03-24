using Autofac;
using NUnit.Framework;
using Snap.Autofac;
using Snap.Tests.Fakes;
using Snap.Tests.Interceptors;

namespace Snap.Tests
{
    [TestFixture]
    public class AutofacTests
    {
        [Test]
        public void Autofac_Container_Supports_Aspects()
        {
            var builder = new ContainerBuilder();

            SnapConfiguration.For(new AutofacAspectContainer(builder)).Configure(c =>
                                                 {
                                                     c.IncludeNamespace("Snap");
                                                     c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                                                 });

            builder.Register(r => new BadCode()).As<IBadCode>();

            using (var container = builder.Build())
            {
                var badCode = container.Resolve<IBadCode>();

                Assert.DoesNotThrow(badCode.GiddyUp);
            }
        }
    }
}
