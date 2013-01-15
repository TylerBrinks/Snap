using System;
using System.Reflection;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Snap;

namespace Snap.CastleWindsor
{ 
    //
    // NOTE: Use this sample as follows: SampleCastleAopConfig.Intercept()
    //
    
    public static class SampleCastleAopConfig
    {
        private readonly static WindsorContainer _container;

        static SampleCastleAopConfig()
        {
            _container = new WindsorContainer();

            SnapConfiguration.For(new CastleAspectContainer(_container.Kernel)).Configure(c =>
            {
                c.IncludeNamespace("ConsoleApplication1");
                c.Bind<SampleInterceptor>().To<SampleAttribute>();
            });

            _container.Register(Component.For(typeof (ISampleClass)).ImplementedBy(typeof (SampleClass)).Named("SampleClass"));
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
