using Castle.Windsor;
using NUnit.Framework;
using Snap.CastleWindsor;
using Snap.Tests.Fakes;
using Snap.Tests.Interceptors;

namespace Snap.Tests
{
    [TestFixture]
    public class CastleWindsorTests
    {
        [Test]
        public void StructureMap_Container_Supports_Aspects()
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
    }
}
