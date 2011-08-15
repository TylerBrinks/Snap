/*
Snap v1.0

Copyright (c) 2010 Tyler Brinks

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using NUnit.Framework;
using Snap.StructureMap;
using SnapTests.Fakes;
using Snap.Tests.Interceptors;
using StructureMap;

namespace Snap.Tests
{
    [TestFixture]
    public class StructureMapTests : TestBase
    {
        [SetUp]
        public void Initialize_Container()
        {
            ObjectFactory.Initialize(x => { });
        }

        [Test]
        public void StructureMap_Container_Supports_Method_Aspects()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
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
                c.IncludeNamespace("SnapTests*");
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
                c.IncludeNamespace("Snap.Tests*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
            });

            ObjectFactory.Configure(c => c.For<INotInterceptable>().Use<NotInterceptable>());
            var code = ObjectFactory.GetInstance<INotInterceptable>();

            Assert.IsFalse(code.GetType().Name.Equals("INotInterceptableProxy"));
        }

        [Test]
        public void Structuremap_Container_Allow_Wildcard_Matching()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            ObjectFactory.Configure(c => c.For<IBadCode>().Use<BadCode>());
            Assert.DoesNotThrow(() => ObjectFactory.GetInstance<IBadCode>());
        }

        [Test]
        public void Structuremap_Container_Allow_Strict_Matching()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            ObjectFactory.Configure(c => c.For<IBadCode>().Use<BadCode>());
            Assert.DoesNotThrow(() => ObjectFactory.GetInstance<IBadCode>());
        }

        [Test]
        public void Structuremap_Container_Succeeds_Without_Match()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("Does.Not.Work");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            ObjectFactory.Configure(c => c.For<IBadCode>().Use<BadCode>());
            Assert.DoesNotThrow(() => ObjectFactory.GetInstance<IBadCode>());
        }

        [Test]
        public void StructureMap_Supports_Types_Without_Interfaces()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            ObjectFactory.Configure( c => c.For<IDependency>().Use<DummyDependency>());

            ObjectFactory.Configure(c => c.For<TypeWithoutInterface>().Use<TypeWithoutInterface>());
            var typeWithoutInterface = ObjectFactory.GetInstance<TypeWithoutInterface>();

            Assert.DoesNotThrow(typeWithoutInterface.Foo);
            Assert.IsTrue(typeWithoutInterface.GetType().Name.Equals("TypeWithoutInterfaceProxy"));

            ObjectFactory.Configure(c => c.For<TypeWithInterfaceInBaseClass>().Use<TypeWithInterfaceInBaseClass>());
            var typeWithInterfaceInBaseClass = ObjectFactory.GetInstance<TypeWithInterfaceInBaseClass>();

            Assert.DoesNotThrow(typeWithInterfaceInBaseClass.Foo);
            Assert.IsTrue(typeWithInterfaceInBaseClass.GetType().Name.Equals("TypeWithInterfaceInBaseClassProxy"));
        }

    }
}
