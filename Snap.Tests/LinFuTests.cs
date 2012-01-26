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
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using LinFu.IoC;
using Snap.LinFu;
using Snap.Tests.Interceptors;
using SnapTests.Fakes;

namespace Snap.Tests
{
    [TestFixture]
    public class LinFuTests : TestBase
    {
        [Test]
        public void LinFu_Container_Supports_Method_Aspects()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            var x = container.GetService<IBadCode>();
            Assert.DoesNotThrow(x.GiddyUp);
        }

        [Test]
        public void LinFu_Container_Supports_Multiple_Method_Aspects()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            var code = container.GetService<IOrderedCode>();
            code.RunInOrder();

            Assert.AreEqual("First", OrderedCode.Actions[0]);
            Assert.AreEqual("Second", OrderedCode.Actions[1]);
        }

        [Test]
        public void LinFu_Container_Supports_Class_Aspects()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<FourthClassInterceptor>().To<FourthClassAttribute>();
            });

            var code = container.GetService<IOrderedCode>();
            code.RunInOrder();

            Assert.AreEqual("Fourth", OrderedCode.Actions[0]);
        }

        [Test]
        public void LinFu_Container_Ignores_Types_Without_Decoration()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
            });

            var code = container.GetService<INotInterceptable>();
          
            Assert.IsFalse(code.GetType().Name.Equals("INotInterceptableProxy"));
        }

        [Test]
        public void LinFu_Container_Allow_Wildcard_Matching()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            Assert.DoesNotThrow(() => container.GetService<IBadCode>());
        }

        [Test]
        public void LinFu_Container_Allow_Strict_Matching()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            Assert.DoesNotThrow(() => container.GetService<IBadCode>());

        }

        [Test]
        public void LinFu_Container_Fails_Without_Match()
        {
            var container = new ServiceContainer();
            container.LoadFrom(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("Does.Not.Work");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            Assert.Throws<NullReferenceException>(() => container.GetService<IBadCode>());
        }

        [Test]
        public void LinFu_Supports_Types_Without_Interfaces()
        {
            var container = new ServiceContainer();

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.Fakes*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.AddService(typeof(IDependency), typeof(DummyDependency));
            container.AddService(typeof(TypeWithoutInterface));
            container.AddService(typeof(TypeWithInterfaceInBaseClass));

            var typeWithoutInterface = container.GetService<TypeWithoutInterface>();
            Assert.DoesNotThrow(typeWithoutInterface.Foo);
            Assert.IsTrue(typeWithoutInterface.GetType().Name.Equals("TypeWithoutInterfaceProxy"));

            var typeWithInterfaceInBaseClass = container.GetService<TypeWithInterfaceInBaseClass>();
            Assert.DoesNotThrow(typeWithInterfaceInBaseClass.Foo);
            Assert.IsTrue(typeWithInterfaceInBaseClass.GetType().Name.Equals("TypeWithInterfaceInBaseClassProxy"));
        }

        [Test]
        public void LinFu_Should_Fail_To_Resolve_Component_Which_Is_Not_Registered_With_ServiceNotFoundException()
        {
            var container = new ServiceContainer();

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
            });

            Assert.Throws<ServiceNotFoundException>(() => container.GetService<IOrderedCode>());
        }

        [Test]
        public void LinFu_Supports_Resolving_All_Aspects_From_Container()
        {
            var container = new ServiceContainer();

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.AllAspects().KeepInContainer();
            });

            // both aspects are registered in container
            container.AddService(typeof(IOrderedCode), typeof(OrderedCode));
            container.AddService(typeof(FirstInterceptor), new FirstInterceptor("first_kept_in_container"));
            container.AddService(typeof(SecondInterceptor), new SecondInterceptor("second_kept_in_container"));

            var orderedCode = container.GetService<IOrderedCode>();
            orderedCode.RunInOrder();

            // both aspects are resolved from container
            CollectionAssert.AreEquivalent(
                OrderedCode.Actions,
                new[] { "first_kept_in_container", "second_kept_in_container" });
        }

        [Test]
        public void LinFu_Supports_Resolving_Only_Selected_Aspects_From_Container()
        {
            var container = new ServiceContainer();

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.Aspects(typeof(FirstInterceptor)).KeepInContainer();
            });

            container.AddService(typeof(IOrderedCode), typeof(OrderedCode));
            container.AddService(typeof(FirstInterceptor), new FirstInterceptor("first_kept_in_container"));
            container.AddService(typeof(SecondInterceptor), new SecondInterceptor("second_kept_in_container"));

            var orderedCode = container.GetService<IOrderedCode>();
            orderedCode.RunInOrder();

            // first interceptor is resolved from container, while second one is via new() 
            CollectionAssert.AreEquivalent(
                OrderedCode.Actions,
                new[] { "first_kept_in_container", "Second" });
        }
        
        [Test]
        public void LinFu_Fails_When_Aspect_Is_Configured_To_Be_Resolved_From_Container_But_Was_Not_Registered_In_It()
        {
            var container = new ServiceContainer();

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespace("SnapTests.*");
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
                c.Aspects(typeof(HandleErrorInterceptor)).KeepInContainer();
            });

            // register only ordered code, but register neither FirstInterceptor nor SecondInteceptor in container
            container.AddService(typeof(IBadCode), typeof(BadCode));

            var failure = Assert.Throws<ActivationException>(container.GetService<IBadCode>().GiddyUp);
            Assert.That(failure.InnerException, Is.InstanceOf<ServiceNotFoundException >());
        }

        [Test]
        [Explicit("No way to unload given assembly from domain w/o destroying domain. Cannot make this test independent from others when all test suite is run.")]
        public void When_resolving_services_from_container_SNAP_should_load_dynamicproxygenassebmly2_in_appdomain_only_once()
        {
            var container = new ServiceContainer();

            SnapConfiguration.For(new LinFuAspectContainer(container)).Configure(c =>
            {
                c.IncludeNamespaceOf<IBadCode>();
                c.Bind<SecondInterceptor>().To<SecondAttribute>();
                c.Bind<FirstInterceptor>().To<FirstAttribute>();
                c.Bind<HandleErrorInterceptor>().To<HandleErrorAttribute>();
            });

            container.AddService(typeof(IBadCode), typeof(BadCode));
            container.AddService(typeof(IOrderedCode), typeof(OrderedCode));

            var orderedCode = container.GetService<IOrderedCode>();
            var badCode = container.GetService<IBadCode>();

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
