using System;
using NUnit.Framework;
using Snap.StructureMap;
using Snap.Tests.Interceptors;
using StructureMap;

namespace Snap.Tests
{
    [TestFixture]
    public class AspectUtilityTests : TestBase
    {
        [Test]
        public void Namespaces_are_included_with_wildcards()
        {
            ObjectFactory.Initialize(i => { });

            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("Snap.Tests*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            ObjectFactory.Configure(c => c.For<Nested.Deeper.IAmNested>().Use<Nested.Deeper.Nested>());
            var badCode = ObjectFactory.GetInstance<Nested.Deeper.IAmNested>();


            Assert.AreEqual("IAmNestedProxy", badCode.GetType().Name, "Not being intercepted");
            Assert.DoesNotThrow(badCode.Foo);
            
        }

        [Test]
        public void Namespaces_are_included_when_specific()
        {

            ObjectFactory.Initialize(i => { });

            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("Snap.Tests.Nested.Deeper");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            ObjectFactory.Configure(c => c.For<Nested.Deeper.IAmNested>().Use<Nested.Deeper.Nested>());
            var badCode = ObjectFactory.GetInstance<Nested.Deeper.IAmNested>();


            Assert.AreEqual("IAmNestedProxy", badCode.GetType().Name, "Not being intercepted");
            Assert.DoesNotThrow(badCode.Foo);
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
