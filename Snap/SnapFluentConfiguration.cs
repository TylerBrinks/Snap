using System;

namespace Snap
{
    public class SnapFluentConfiguration : IHideBaseTypes
    {
        private readonly IAspectConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapFluentConfiguration"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public SnapFluentConfiguration(IAspectConfiguration config)
        {
            _configuration = config;
        }

        /// <summary>
        /// Configures the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Configure(Action<IAspectConfiguration> configuration)
        {
            configuration(_configuration);
        }
    }
}