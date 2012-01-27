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
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Snap.CastleWindsor;
using SnapTests.Fakes;
using Snap.Tests.Interceptors;

namespace Snap.Tests
{
    [TestFixture]
    public class CastleWindsorTests : TestBase
    {
        [Test]
        public void CastleWindsor_Container_Supports_Method_Aspects()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
                                {
                                    c.IncludeNamespace("SnapTests*");
                                    c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                                });

            container.Register(Component.For(typeof (IBadCode)).ImplementedBy(typeof (BadCode)).Named("BadCode"));
            var badCode = container.Resolve<IBadCode>();
            Assert.DoesNotThrow(badCode.GiddyUp);
        }

        [Test]
        public void CastleWindsor_Container_Supports_Multiple_Method_Aspects()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            container.Register(
                Component.For(typeof (IOrderedCode)).ImplementedBy(typeof (OrderedCode)).Named("OrderedCode"));
            var orderedCode = container.Resolve<IOrderedCode>();
            orderedCode.RunInOrder();
            Assert.AreEqual("First", OrderedCode.Actions[0]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
        }

        [Test]
        public void CastleWindsor_Container_Supports_Class_Aspects()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<FourthClassInterceptor>().To<FourthClassAttribute>();
            });

            container.Register(
                Component.For(typeof(IOrderedCode)).ImplementedBy(typeof(ClassOrderedCode)).Named("OrderedCode"));
            var code = container.Resolve<IOrderedCode>();
            code.RunInOrder();

            Assert.AreEqual("Fourth", OrderedCode.Actions[0]);
        }

        [Test]
        public void CastleWindsor_Supports_Types_Without_Interfaces()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Register(Component.For<IDependency>().ImplementedBy<DummyDependency>());
            container.Register(Component.For<TypeWithoutInterface>().ImplementedBy<TypeWithoutInterface>());
            container.Register(Component.For<TypeWithInterfaceInBaseClass>().ImplementedBy<TypeWithInterfaceInBaseClass>());

            var typeWithoutInterface = container.Resolve<TypeWithoutInterface>();
            Assert.DoesNotThrow(typeWithoutInterface.Foo);
            Assert.IsTrue(typeWithoutInterface.GetType().Name.Equals("TypeWithoutInterfaceProxy"));

            var typeWithInterfaceInBaseClass = container.Resolve<TypeWithInterfaceInBaseClass>();
            Assert.DoesNotThrow(typeWithInterfaceInBaseClass.Foo);
            Assert.IsTrue(typeWithInterfaceInBaseClass.GetType().Name.Equals("TypeWithInterfaceInBaseClassProxy"));
        }

        [Test]
        public void CastleWindsor_Supports_Resolving_All_Aspects_From_Container()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.AllAspects().KeepInContainer();
            });

            container.Register(Component.For(typeof(IOrderedCode)).ImplementedBy(typeof(OrderedCode)));
            container.Register(Component.For<FirstInterceptor>().Instance(new FirstInterceptor("first_kept_in_container")));
            container.Register(Component.For<SecondInterceptor>().Instance(new SecondInterceptor("second_kept_in_container")));

            var orderedCode = container.Resolve<IOrderedCode>();
            orderedCode.RunInOrder();

            // both interceptors are resolved from container
            CollectionAssert.AreEquivalent(
                OrderedCode.Actions,
                new[] { "first_kept_in_container", "second_kept_in_container" });
            
        }

        [Test]
        public void CastleWindsor_Supports_Resolving_Only_Selected_Aspects_From_Container()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.Aspects(typeof(FirstInterceptor)).KeepInContainer();
            });

            container.Register(Component.For(typeof(IOrderedCode)).ImplementedBy(typeof(OrderedCode)));
            container.Register(Component.For<FirstInterceptor>().Instance(new FirstInterceptor("first_kept_in_container")));
            container.Register(Component.For<SecondInterceptor>().Instance(new SecondInterceptor("second_kept_in_container")));

            var orderedCode = container.Resolve<IOrderedCode>();
            orderedCode.RunInOrder();

            // first interceptor is resolved from container, while second one is via new() 
            CollectionAssert.AreEquivalent(
                OrderedCode.Actions,
                new[] { "first_kept_in_container", "Second" });
        }

        [Test]
        public void CastleWindsor_Fails_When_Aspect_Is_Configured_To_Be_Resolved_From_Container_But_Was_Not_Registered_In_It()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                c.Aspects(typeof(HandleErrorInterceptor)).KeepInContainer();
            });

            // register only bad code, but do not register HandleErrorInterceptor in container
            container.Register(Component.For(typeof(IBadCode)).ImplementedBy(typeof(BadCode)));

            var codeWithSingleAspect = container.Resolve<IBadCode>();
            var failure = Assert.Throws<ActivationException>(codeWithSingleAspect.GiddyUp);
            Assert.That(failure.InnerException, Is.InstanceOf<ComponentNotFoundException>());
        }

        [Test]
        [Explicit("No way to unload given assembly from domain w/o destroying domain. Cannot make this test independent from others when all test suite is run.")]
        public void When_resolving_services_from_container_SNAP_should_load_dynamicproxygenassebmly2_in_appdomain_only_once()
        {
            var container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(container.Kernel)).Configure(c =>
            {
                c.IncludeNamespaceOf<IBadCode>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.Register(Component.For(typeof(IBadCode)).ImplementedBy(typeof(BadCode)));
            container.Register(Component.For(typeof(IOrderedCode)).ImplementedBy(typeof(OrderedCode)));

            var orderedCode = container.Resolve<IOrderedCode>();
            var badCode = container.Resolve<IBadCode>();

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
