using Autofac;
using Castle.Core.Interceptor;

namespace Snap.Autofac
{
    /// <summary>
    /// Autofac Aspect Container for AoP interception registration.
    /// </summary>
    public class AutofacAspectContainer : IAspectContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacAspectContainer"/> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public AutofacAspectContainer(ContainerBuilder builder)
        {
            Builder = builder;
            builder.RegisterModule<AutofacAspectModule>();
        }
 
        /// <summary>
        /// Gets or sets the container builder.
        /// </summary>
        /// <value>The builder.</value>
        public ContainerBuilder Builder { get; set; }

        /// <summary>
        /// Sets the aspect configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        public void SetConfiguration(AspectConfiguration config)
        {
            Builder.RegisterInstance(config);
            config.Container = this;
        }
        /// <summary>
        /// Registers a method interceptor.
        /// </summary>
        /// <typeparam name="T">Interceptor type</typeparam>
        public void Bind<T>() where T : IInterceptor, new()
        {
            var type = typeof (T);
            Builder.RegisterType<T>().As<IInterceptor>().Named(type.FullName, typeof(IInterceptor));
        }
    }
}
