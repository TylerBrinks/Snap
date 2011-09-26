using System;

namespace Snap
{
    /// <summary>
    /// Entry point to configure set of registered aspects in a single step. 
    /// It's a place where any configuration for a set of aspects (aspect book) is applicable.
    /// </summary>
    public interface IAspectBookSyntax
    {
        /// <summary>
        /// Configure aspects to be resolved from container.
        /// </summary>
        /// <remarks>
        /// Of course, also you should register aspects in container
        /// </remarks>>
        void KeepInContainer();
    }
}