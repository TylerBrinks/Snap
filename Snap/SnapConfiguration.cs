using System;

namespace Snap
{
    /// <summary>
    /// Entry point for configuring Snap AoP providers
    /// </summary>
    public static class SnapConfiguration
    {
        /// <summary>
        /// Specifies configuration for a type of configuration container.
        /// </summary>
        /// <typeparam name="T">Type of configuration container.</typeparam>
        /// <param name="configuration">The configuration action.</param>
        public static void For<T>(Action<IAspectConfiguration> configuration) where T : IAspectContainer, new()
        {
            var container = new T();

            For(container).Configure(configuration);
        }
        /// <summary>
        /// Specifies configuration for a configuration container.
        /// </summary>
        /// <param name="container">The AoP container.</param>
        public static SnapFluentConfiguration For(IAspectContainer container)
        {
            var config = new AspectConfiguration();
            container.SetConfiguration(config);

            return new SnapFluentConfiguration(config);
        }
    }
}
