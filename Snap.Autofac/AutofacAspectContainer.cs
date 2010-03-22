using System;
using Autofac;
using Castle.Core.Interceptor;

namespace Snap.Autofac
{
    public class AutofacAspectContainer : IAspectContainer
    {
        public AutofacAspectContainer(ContainerBuilder builder)
        {
            Builder = builder;
            builder.RegisterModule<AutofacAspectModule>();
        }

        public void SetConfiguration(AspectConfiguration config)
        {
            Builder.RegisterInstance(config);
            config.Container = this;
        }

        public void RegisterInterceptor<T>() where T : IInterceptor, new()
        {
            var type = typeof (T);
            Builder.RegisterType<T>().As<IInterceptor>().Named(type.FullName, typeof(IInterceptor));
        }

        public ContainerBuilder Builder { get; set; }
    }
}
