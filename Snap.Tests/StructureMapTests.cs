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
        //[Test]
        //public void StructureMap_Runs_Method_Interceptors_In_Decorated_Order()
        //{
        //    SnapConfiguration.For<StructureMapAspectContainer>(c =>
        //    {
        //        c.IncludeNamespace("Snap.Tests");
        //        c.Bind<FirstInterceptor>().To<FirstAttribute>();
        //        c.Bind<SecondInterceptor>().To<SecondAttribute>();
        //    });

        //    ObjectFactory.Configure(c => c.For<IOrderedCode>().Use<OrderedCode>());
        //    var code = ObjectFactory.GetInstance<IOrderedCode>();
        //    code.RunInDecoratedOrder();

        //    Assert.AreEqual("First", OrderedCode.Actions[1]);
        //    Assert.AreEqual("Second", OrderedCode.Actions[0]);
        //}
    }
}
