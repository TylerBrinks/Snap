using NUnit.Framework;
using Snap.StructureMap;
using Snap.Tests.Fakes;
using Snap.Tests.Interceptors;
using StructureMap;

namespace Snap.Tests
{
    [TestFixture]
    public class StructureMapTests
    {
        [Test]
        public void StructureMap_Container_Supports_Aspects()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
                                                      {
                                                          c.IncludeNamespace("Snap.Tests");
                                                          c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                                                      });

            ObjectFactory.Configure(c => c.For<IBadCode>().Use<BadCode>());
            var badCode = ObjectFactory.GetInstance<IBadCode>();
            Assert.DoesNotThrow(badCode.GiddyUp);
        }
    }
}
