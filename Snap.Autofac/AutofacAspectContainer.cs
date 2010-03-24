using Autofac;

namespace Snap.Autofac
{
    /// <summary>
    /// Autofac Aspect Container for AoP interception registration.
    /// </summary>
    public class AutofacAspectContainer : AspectContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacAspectContainer"/> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public AutofacAspectContainer(ContainerBuilder builder)
        {
            Proxy = new MasterProxy();
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
        public override void SetConfiguration(AspectConfiguration config)
        {
            Proxy.Configuration = config;
            Builder.RegisterInstance((MasterProxy)Proxy);
            config.Container = this;
        }
    }
}
