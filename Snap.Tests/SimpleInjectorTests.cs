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
using System.Linq;
using NUnit.Framework;
using Snap.SimpleInjector;
using SnapTests.Fakes;
using Snap.Tests.Interceptors;
using SimpleInjector;

namespace Snap.Tests
{
    [TestFixture]
    public class SimpleInjectorTests : TestBase
    {
        [Test]
        public void SimpleInjector_Container_Supports_Method_Aspects()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("SnapTests*");
                                                               c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                                                           });

            container.Container.Register<IBadCode, BadCode>();
            var badCode = container.Container.GetInstance<IBadCode>();

            Assert.DoesNotThrow(badCode.GiddyUp);
            Assert.IsTrue(badCode.GetType().Name.Equals("IBadCodeProxy"));
        }

        [Test]
        public void SimpleInjector_Container_Supports_Multiple_Method_Aspects()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("SnapTests*");
                                                               c.Bind<FirstInterceptor>().To<FirstAttribute>();
                                                               c.Bind<SecondInterceptor>().To<SecondAttribute>();
                                                           });

            container.Container.Register<IOrderedCode, OrderedCode>();
            var badCode = container.Container.GetInstance<IOrderedCode>();
            badCode.RunInOrder();
            Assert.AreEqual("First", OrderedCode.Actions[0]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
        }

        [Test]
        public void SimpleInjector_Container_Supports_Class_Aspects()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("SnapTests*");
                                                               c.Bind<FourthClassInterceptor>().To<FourthClassAttribute>
                                                                   ();
                                                           });

            container.Container.Register<IOrderedCode, ClassOrderedCode>();
            var code = container.Container.GetInstance<IOrderedCode>();
            code.RunInOrder();

            Assert.AreEqual("Fourth", OrderedCode.Actions[0]);
        }

        [Test]
        public void SimpleInjector_Container_Ignores_Types_Without_Decoration()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("SnapTests*");
                                                               c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>
                                                                   ();
                                                           });

            container.Container.Register<INotInterceptable, NotInterceptable>();
            var code = container.Container.GetInstance<INotInterceptable>();

            Assert.IsFalse(code.GetType().Name.Equals("INotInterceptableProxy"));
        }

        [Test]
        public void SimpleInjector_Container_Allow_Wildcard_Matching()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("SnapTests*");
                                                               c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>
                                                                   ();
                                                           });

            container.Container.Register<IBadCode, BadCode>();
            var badCode = container.Container.GetInstance<IBadCode>();

            Assert.DoesNotThrow(badCode.GiddyUp);
            Assert.IsTrue(badCode.GetType().Name.Equals("IBadCodeProxy"));
        }

        [Test]
        public void SimpleInjector_Container_Allow_Strict_Matching()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("SnapTests.Fakes.IBadCode");
                                                               c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>
                                                                   ();
                                                           });

            container.Container.Register<IBadCode, BadCode>();
            Assert.DoesNotThrow(() => container.Container.GetInstance<IBadCode>());
        }

        [Test]
        public void SimpleInjector_Container_Fails_Without_Match()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("Does.Not.Work");
                                                               c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>
                                                                   ();
                                                           });

            container.Container.Register<IBadCode, BadCode>();
            Assert.Throws<ActivationException>(() => container.Container.GetInstance<IBadCode>());
        }

        [Test]
        public void SimpleInjector_Supports_Types_Without_Interfaces()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("SnapTests.Fakes*");
                                                               c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                                                           });

            container.Container.Register<IDependency, DummyDependency>();

            container.Container.Register<TypeWithoutInterface, TypeWithoutInterface>();
            container.Container.Register<TypeWithInterfaceInBaseClass, TypeWithInterfaceInBaseClass>();

            var typeWithoutInterface = container.Container.GetInstance<TypeWithoutInterface>();

            Assert.DoesNotThrow(typeWithoutInterface.Foo);
            Assert.IsTrue(typeWithoutInterface.GetType().Name.Equals("TypeWithoutInterfaceProxy"));
                        
            var typeWithInterfaceInBaseClass = container.Container.GetInstance<TypeWithInterfaceInBaseClass>();

            Assert.DoesNotThrow(typeWithInterfaceInBaseClass.Foo);
            Assert.IsTrue(typeWithInterfaceInBaseClass.GetType().Name.Equals("TypeWithInterfaceInBaseClassProxy"));
        }

        [Test]
        public void SimpleInjector_Supports_Resolving_All_Aspects_From_Container()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("SnapTests.*");
                                                               c.Bind<FirstInterceptor>().To<FirstAttribute>();
                                                               c.Bind<SecondInterceptor>().To<SecondAttribute>();
                                                               c.AllAspects().KeepInContainer();
                                                           });

            container.Container.Register<IOrderedCode, OrderedCode>();
            container.Container.RegisterSingle<FirstInterceptor>(new FirstInterceptor("first_kept_in_container"));
            container.Container.RegisterSingle<SecondInterceptor>(new SecondInterceptor("second_kept_in_container"));

            var orderedCode = container.Container.GetInstance<IOrderedCode>();
            orderedCode.RunInOrder();

            // both interceptors are resolved from container
            CollectionAssert.AreEquivalent(
                OrderedCode.Actions,
                new[] {"first_kept_in_container", "second_kept_in_container"});
        }

        [Test]
        public void SimpleInjector_Supports_Resolving_Only_Selected_Aspects_From_Container()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespace("SnapTests.*");
                                                               c.Bind<FirstInterceptor>().To<FirstAttribute>();
                                                               c.Bind<SecondInterceptor>().To<SecondAttribute>();
                                                               c.Aspects(typeof (FirstInterceptor)).KeepInContainer();
                                                           });

            container.Container.Register<IOrderedCode, OrderedCode>();
            container.Container.RegisterSingle<FirstInterceptor>(new FirstInterceptor("first_kept_in_container"));
            container.Container.RegisterSingle<SecondInterceptor>(new SecondInterceptor("second_kept_in_container"));

            var orderedCode = container.Container.GetInstance<IOrderedCode>();
            orderedCode.RunInOrder();

            // first interceptor is resolved from container, while second one is via new() 
            CollectionAssert.AreEquivalent(
                OrderedCode.Actions,
                new[] {"first_kept_in_container", "Second"});
        }

        [Test]
        [Explicit(
            "No way to unload given assembly from domain w/o destroying domain. Cannot make this test independent from others when all test suite is run."
            )]
        public void
            When_resolving_services_from_container_SNAP_should_load_dynamicproxygenassebmly2_in_appdomain_only_once()
        {
            var container = new SimpleInjectorAspectContainer(new Container());

            SnapConfiguration.For(container).Configure(c =>
                                                           {
                                                               c.IncludeNamespaceOf<IBadCode>();
                                                               c.Bind<SecondInterceptor>().To<SecondAttribute>();
                                                               c.Bind<FirstInterceptor>().To<FirstAttribute>();
                                                               c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                                                           });

            container.Container.Register<IOrderedCode, OrderedCode>();
            container.Container.Register<IBadCode, BadCode>();

            var orderedCode = container.Container.GetInstance<IOrderedCode>();
            var badCode = container.Container.GetInstance<IBadCode>();

            orderedCode.RunInOrder();
            Assert.AreEqual("First", OrderedCode.Actions[1]);
            Assert.AreEqual("Second", OrderedCode.Actions[0]);

            Assert.DoesNotThrow(badCode.GiddyUp);

            var dynamicProxyGenerationAssemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => assembly.GetName().Name == "DynamicProxyGenAssembly2")
                .ToList();

            Assert.That(dynamicProxyGenerationAssemblies.Count, Is.EqualTo(2));
            // both signed and unsigned.
            Assert.IsNotNull(dynamicProxyGenerationAssemblies.FirstOrDefault(a => a.GetName().GetPublicKey().Length > 0));
            Assert.IsNotNull(dynamicProxyGenerationAssemblies.FirstOrDefault(a => a.GetName().GetPublicKey().Length == 0));
        }
    }
}