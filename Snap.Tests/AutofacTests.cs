using Autofac;
using NUnit.Framework;
using Snap.Autofac;
using Snap.Tests.Fakes;
using Snap.Tests.Interceptors;

namespace Snap.Tests
{
    [TestFixture]
    public class AutofacTests : TestBase
    {
        [Test]
        public void Autofac_Container_Supports_Method_Aspects()
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
        [Test]
        public void Autofac_Container_Supports_Multiple_Method_Aspects()
        {
            var builder = new ContainerBuilder();

            SnapConfiguration.For(new AutofacAspectContainer(builder)).Configure(c =>
            {
                c.IncludeNamespace("Snap");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            builder.Register(r => new OrderedCode()).As<IOrderedCode>();

            using (var container = builder.Build())
            {
                var orderedCode = container.Resolve<IOrderedCode>();
                orderedCode.RunInOrder();

                Assert.AreEqual("First", OrderedCode.Actions[0]);
                Assert.AreEqual("Second", OrderedCode.Actions[1]);
            }
        }
    }
}
