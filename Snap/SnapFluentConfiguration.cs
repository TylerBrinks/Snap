using System;

namespace Snap
{
    public class SnapFluentConfiguration
    {
        private readonly AspectConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapFluentConfiguration"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public SnapFluentConfiguration(AspectConfiguration config)
        {
            _configuration = config;
        }

        /// <summary>
        /// Configures the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Configure(Action<AspectConfiguration> configuration)
        {
            configuration(_configuration);
        }
    }
}