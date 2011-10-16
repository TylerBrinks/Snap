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

using Ninject;
using NUnit.Framework;
using Snap;
using Snap.Ninject;
using Snap.StructureMap;
using Snap.Tests;
using Snap.Tests.Fakes;
using Snap.Tests.Interceptors;
using SnapTests.Fakes;
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
        public void Type_Inclusion_Adds_Type_By_Names()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeType("SnapTests.Fakes.IBadCode");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            Assert.DoesNotThrow(() => container.Kernel.Get<IBadCode>());
        }
        [Test]
        public void Type_Inclusion_Adds_Type_By_Interface()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeType(typeof(IBadCode));
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            Assert.DoesNotThrow(() => container.Kernel.Get<IBadCode>());
        }
        [Test]
        public void Type_Inclusion_Adds_Type_By_Generic()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeType<IBadCode>();
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            Assert.DoesNotThrow(() => container.Kernel.Get<IBadCode>());
        }
        [Test]
        public void Namespace_Inclusion_Adds_Wildcards()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                c.IncludeNamespace("SnapTests");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            Assert.DoesNotThrow(() => container.Kernel.Get<IBadCode>());
        }
        [Test]
        public void Namespace_Inclusion_Checks_For_Wildcards()
        {
            var container = new MockAspectContainer();

            SnapConfiguration.For(container).Configure(c => c.IncludeNamespace("SnapTests*"));

            var config = container.AspectConfiguration;
            Assert.IsTrue(config.Namespaces[0].Equals("SnapTests*"));
        }
        [Test]
        public void Snap_Scans_Assemblies_For_Default_Conventions()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTest*");
                c.Scan(s => s.ThisAssembly().WithDefaults());
            });

            ObjectFactory.Configure(c => c.For<IBadCode>().Use<BadCode>());
            var code = ObjectFactory.GetInstance<IBadCode>();

            Assert.DoesNotThrow(code.GiddyUp);
        }
        [Test]
        public void Snap_Allows_Custom_Scanning_Conventions()
        {
            var scanner = new CustomPrefixScanner();

            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("SnapTest*");
                c.Scan(s => s.ThisAssembly().With(scanner));
            });

            ObjectFactory.Configure(c => c.For<IBadCode>().Use<BadCode>());
            var code = ObjectFactory.GetInstance<IBadCode>();

            Assert.DoesNotThrow(code.GiddyUp);
        }

        [Test]
        public void Namespace_Inclusion_Adds_Namespace_Of_A_Given_Type()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                // have the same namespace as IBadCode
                c.IncludeNamespaceOf<DummyDependency>();
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            Assert.DoesNotThrow(() => container.Kernel.Get<IBadCode>());
        }

        [Test]
        public void Namespace_Inclusion_Adds_Namespace_and_Nested_Namespaces_Of_A_Given_Type()
        {
            var container = new NinjectAspectContainer();

            SnapConfiguration.For(container).Configure(c =>
            {
                // AspectConfiguration have the same namespace prefix as IBadCode's namespace
                c.IncludeNamespaceOf<AspectConfiguration>(true);
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Kernel.Bind<IBadCode>().To<BadCode>();
            Assert.DoesNotThrow(() => container.Kernel.Get<IBadCode>());
        }

        //[Test]
        //public void Wildcard_at_root_works_as_expected()
        //{
        //    var namespaces = new List<string> { "SnapTests*" };
        //    var types = new List<Type> { typeof(IBadCode) }.ToArray();

        //    Assert.AreEqual(typeof(IBadCode), types.FirstMatch(namespaces));
        //}

        //[Test]
        //public void Wildcard_at_second_level_works_as_expected()
        //{
        //    var namespaces = new List<string> { "SnapTests.Fakes*" };
        //    var types = new List<Type> { typeof(IBadCode) }.ToArray();

        //    Assert.AreEqual(typeof(IBadCode), types.FirstMatch(namespaces));
        //}

        //[Test]
        //public void Explicity_naming_a_type_as_an_namespace_to_include_works_but_perhaps_should_not()
        //{
        //    var namespaces = new List<string> { "SnapTests.Fakes.IBadCode" };
        //    var types = new List<Type> { typeof(IBadCode) }.ToArray();

        //    Assert.AreEqual(typeof(IBadCode), types.FirstMatch(namespaces));
        //}

        //[Test]
        //public void Explicitly_naming_a_namespace_does_not_work_but_should()
        //{
        //    var namespaces = new List<string> { "SnapTests.Fakes" };
        //    var types = new List<Type> { typeof(IBadCode) }.ToArray();

        //    Assert.AreEqual(typeof(IBadCode), types.FirstMatch(namespaces));
        //}
    }
}
