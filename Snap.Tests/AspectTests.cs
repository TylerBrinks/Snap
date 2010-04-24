using System;
using Ninject;
using NUnit.Framework;
using Snap.Ninject;
using Snap.StructureMap;
using Snap.Tests.Fakes;
using SnapTests.Fakes;
using Snap.Tests.Interceptors;
using StructureMap;

namespace Snap.Tests
{
    [TestFixture]
    public class AspectTests : TestBase
    {
        [Test]
        public void Method_Interceptors_Run_In_Configured_Order_By_Default()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
            });

            ObjectFactory.Configure(c => c.For<IOrderedCode>().Use<OrderedCode>());
            var code = ObjectFactory.GetInstance<IOrderedCode>();

            code.RunInOrder();

            Assert.AreEqual("First", OrderedCode.Actions[1]);
            Assert.AreEqual("Second", OrderedCode.Actions[0]);
        }
        [Test]
        public void Method_Interceptors_Run_In_Order_When_Set_Explicitly()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
                c.Bind<SecondInterceptor>().To<SecondAttribute>().Order(2);
                c.Bind<FirstInterceptor>().To<FirstAttribute>().Order(3);
                c.Bind<ThirdInterceptor>().To<ThirdAttribute>().Order(1);
            });

            ObjectFactory.Configure(c => c.For<IOrderedCode>().Use<OrderedCode>());
            var code = ObjectFactory.GetInstance<IOrderedCode>();
            code.RunInExplicitOrder();

            Assert.AreEqual("First", OrderedCode.Actions[2]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
            Assert.AreEqual("Third", OrderedCode.Actions[0]);
        }
        [Test]
        public void Method_Interceptors_Run_In_Order_When_Attributed()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<ThirdInterceptor>().To<ThirdAttribute>();
            });

            ObjectFactory.Configure(c => c.For<IOrderedCode>().Use<OrderedCode>());
            var code = ObjectFactory.GetInstance<IOrderedCode>();
            code.RunInAttributedOrder();

            Assert.AreEqual("First", OrderedCode.Actions[1]);
            Assert.AreEqual("Second", OrderedCode.Actions[2]);
            Assert.AreEqual("Third", OrderedCode.Actions[0]);
        }
        [Test]
        public void Namespace_Inclusion_Adds_Wildcards()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespaceRoot("SnapTests");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            Assert.DoesNotThrow(() => container.Kernel.Get<IBadCode>());
        }
        [Test]
        public void Namespace_Inclusion_Checks_For_Wildcards()
        {
            var container = new MockAspectContainer();
            
            SnapConfiguration.For(container).Configure(c => c.IncludeNamespaceRoot("SnapTests*"));

            var config = container.AspectConfiguration;
            Assert.IsTrue(config.Namespaces[0].Equals("SnapTests*"));
        }
    }
}
