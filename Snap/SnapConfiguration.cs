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
        public static void For<T>(Action<AspectConfiguration> configuration) where T : IAspectContainer, new()
        {
            //var config = new AspectConfiguration();
            var container = new T();
            //container.SetConfiguration(config);

            //configuration(config);

            For(container, configuration);
        }
        /// <summary>
        /// Specifies configuration for a configuration container.
        /// </summary>
        /// <param name="container">The AoP container.</param>
        /// <param name="configuration">The configuration action.</param>
        public static void For(IAspectContainer container, Action<AspectConfiguration> configuration)
        {
            var config = new AspectConfiguration();
            container.SetConfiguration(config);
            configuration(config);
        }
    }
}
