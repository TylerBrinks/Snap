using System;
using NUnit.Framework;
using Snap.StructureMap;
using Snap.Tests.Interceptors;
using SnapTests.Fakes;
using StructureMap;

namespace Snap.Tests
{
    [TestFixture]
    public class AspectUtilityTests : TestBase
    {
        [Test]
        public void Namespaces_are_included_with_wildcards()
        {
            //ObjectFactory.Initialize(i => { });

            //SnapConfiguration.For<StructureMapAspectContainer>(c =>
            //{
            //    c.IncludeNamespace("SnapTests.Nested*");
            //    c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            //});

            //ObjectFactory.Configure(c => c.For<Nested.Deeper.IAmNested>().Use<Nested.Deeper.Nested>());
            //var nested = ObjectFactory.GetInstance<Nested.Deeper.IAmNested>();

            //Assert.AreEqual("IBadCodeProxy", nested.GetType().Name, "Not being intercepted");
            //Assert.DoesNotThrow(nested.Foo);

            ObjectFactory.Initialize(i => { });

            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            ObjectFactory.Configure(c => c.For<IBadCode>().Use<BadCode>());
            var badCode = ObjectFactory.GetInstance<IBadCode>();


            Assert.AreEqual("IBadCodeProxy", badCode.GetType().Name, "Not being intercepted");
            Assert.DoesNotThrow(badCode.GiddyUp);
            
        }

        [Test]
        public void Namespaces_are_included_when_specific()
        {

            ObjectFactory.Initialize(i => { });

            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            ObjectFactory.Configure(c => c.For<IBadCode>().Use<BadCode>());
            var badCode = ObjectFactory.GetInstance<IBadCode>();


            Assert.AreEqual("IBadCodeProxy", badCode.GetType().Name, "Not being intercepted");
            Assert.DoesNotThrow(badCode.GiddyUp);
        }
    }

    namespace Nested.Deeper
    {
        public interface IAmNested
        {
            void Foo();
        }

        public class Nested : IAmNested
        {
            [HandleError]
            public void Foo()
            {
                throw new Exception("Interceptor will catch this");
            }
        }
    }


}
