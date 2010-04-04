using Castle.Windsor;
using NUnit.Framework;
using Snap.CastleWindsor;
using Snap.Tests.Fakes;
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
                                    c.IncludeNamespace("Snap.Tests");
                                    c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                                });

            container.AddComponent("BadCode", typeof (IBadCode), typeof (BadCode));

            var badCode = (IBadCode)container.Kernel[typeof(IBadCode)];
            Assert.DoesNotThrow(badCode.GiddyUp);
        }

        [Test]
        public void CastleWindsor_Container_Supports_Multiple_Method_Aspects()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
            {
                c.IncludeNamespace("Snap.Tests");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            container.AddComponent("OrderedCode", typeof(IOrderedCode), typeof(OrderedCode));

            var orderedCode = (IOrderedCode)container.Kernel[typeof(IOrderedCode)];
            orderedCode.RunInOrder();
            Assert.AreEqual("First", OrderedCode.Actions[0]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
        }
    }
}
