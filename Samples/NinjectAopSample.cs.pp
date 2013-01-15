using System;
using System.Reflection;
using Castle.DynamicProxy;
using Ninject;
using Snap.Ninject;
using Snap;

namespace ConsoleApplication1
{   
    //
    // NOTE: Use this sample as follows: SampleNinjectAopConfiguration.Intercept()
    //
    
    public static class SampleNinjectAopConfiguration
    {
        public readonly static NinjectAspectContainer _container;

        static SampleNinjectAopConfiguration()
        { 
            _container = new NinjectAspectContainer();

            SnapConfiguration.For(_container).Configure(c =>
            {
                c.IncludeNamespace("ConsoleApplication1*");
                c.Bind<SampleInterceptor>().To<SampleAttribute>();
            });

            _container.Kernel.Bind<ISampleClass>().To<SampleClass>();
        }

        public static void Intercept()
        {
            var instance = _container.Kernel.Get<ISampleClass>();
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
