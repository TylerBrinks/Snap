using System;
using System.Reflection;
using Autofac;
using Castle.DynamicProxy;
using Snap;
using Snap.Autofac;

namespace $rootnamespace$
{
    public static class SampleAutofacAopConfig
    {
        private readonly static ContainerBuilder _builder;

        static SampleAutofacAopConfig()
        {
            _builder = new ContainerBuilder();

            SnapConfiguration.For(new AutofacAspectContainer(_builder)).Configure(c =>
            {
                c.IncludeNamespaceRoot("$rootnamespace$");
                c.Bind<SampleInterceptor>().To<SampleAttribute>();
            });

            _builder.Register(r => new SampleClass()).As<ISampleClass>();
        }

        public static void Intercept()
        {
            using (var container = _builder.Build())
            {
                var instance = container.Resolve<ISampleClass>();
                instance.Run();
            }
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
