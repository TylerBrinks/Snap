
using System;
using System.Reflection;
using Castle.DynamicProxy;
using Snap.StructureMap;
using StructureMap;
using Snap;

namespace ConsoleApplication1
{
    //
    // NOTE: Use this sample as follows: SampleStructureMapAopConfig.Intercept()
    //
    
    public static class SampleStructureMapAopConfig
    {
        static SampleStructureMapAopConfig()
        {
            SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("ConsoleApplication1*");
                c.Bind<SampleInterceptor>().To<SampleAttribute>();
            });

            ObjectFactory.Configure(c => c.For<ISampleClass>().Use<SampleClass>());
        }

        public static void Intercept()
        {
            var instance = ObjectFactory.GetInstance<ISampleClass>();
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
