
namespace Snap
{
    /// <summary>
    /// Syntax for fluent configuration
    /// </summary>
    /// <typeparam name="T">Type of interceptor being configured</typeparam>
    public class ConfigurationSyntax<T> : IConfigurationSyntax
    {
        private readonly AspectConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSyntax&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ConfigurationSyntax(AspectConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Binds an attribute to an interceptor.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        public void To<TAttribute>()
        {
            _configuration.BindInterceptor<T, TAttribute>();
        }
    }
}
