using NUnit.Framework;
using Snap.StructureMap;
using Snap.Tests.Fakes;
using Snap.Tests.Interceptors;
using StructureMap;

namespace Snap.Tests
{
    [TestFixture]
    public class StructureMapTests : TestBase
    {
        [Test]
        public void StructureMap_Container_Supports_Method_Aspects()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("Snap.Tests");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            ObjectFactory.Configure(c => c.For<IBadCode>().Use<BadCode>());
            var badCode = ObjectFactory.GetInstance<IBadCode>();
            
            Assert.DoesNotThrow(badCode.GiddyUp);
            Assert.IsTrue(badCode.GetType().Name.Equals("IBadCodeProxy"));
        }
        [Test]
        public void StructureMap_Container_Supports_Multiple_Method_Aspects()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("Snap.Tests");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            ObjectFactory.Configure(c => c.For<IOrderedCode>().Use<OrderedCode>());
            var code = ObjectFactory.GetInstance<IOrderedCode>();
            code.RunInOrder();

            Assert.AreEqual("First", OrderedCode.Actions[0]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
        }
        [Test]
        public void Structuremap_Container_Ignores_Types_Without_Decoration()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("Snap.Tests");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
            });

            ObjectFactory.Configure(c => c.For<INotInterceptable>().Use<NotInterceptable>());
            var code = ObjectFactory.GetInstance<INotInterceptable>();

            Assert.IsFalse(code.GetType().Name.Equals("INotInterceptableProxy"));
        }
    }
}
