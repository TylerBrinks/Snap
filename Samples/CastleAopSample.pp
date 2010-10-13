using System;
using System.Reflection;
using Castle.Core.Interceptor;
using Castle.Windsor;

namespace Snap.CastleWindsor
{
    public static class SampleCastleAopConfig
    {
        private readonly static WindsorContainer _container;

        static SampleCastleAopConfig()
        {
            _container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(_container.Kernel)).Configure(c =>
            {
                c.IncludeNamespaceRoot("$rootnamespace$");
                c.Bind<SampleInterceptor>().To<SampleAttribute>();
            });

            _container.AddComponent("SampleClass", typeof(ISampleClass), typeof(SampleClass));
        }

        public static void Intercept()
        {
            var instance = (ISampleClass)_container.Kernel[typeof(ISampleClass)];
            instance.Run();
        }
    }

    public interface ISampleClass
    {
        void Run();
    }

    public class SampleClass : ISampleClass
    {
        [Sample] // Don't forget your attribute!
        public void Run()
        {
            Console.WriteLine("inside the method");
        }
    }

    public class SampleInterceptor : MethodInterceptor
    {
        public override void BeforeInvocation()
        {
            Console.WriteLine("this is executed before your method");
            base.BeforeInvocation();
        }

        public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
        {
            // Just keep running for this demo.  
            invocation.Proceed(); // the underlying method call
        }

        public override void AfterInvocation()
        {
            Console.WriteLine("this is executed after your method");
            base.AfterInvocation();
        }
    }

    public class SampleAttribute : MethodInterceptAttribute
    { }
}
