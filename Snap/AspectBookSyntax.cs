using System.Collections.Generic;
using System.Linq;

namespace Snap
{
    /// <summary>
    /// Entry point to configure set of registered aspects in a single step. 
    /// It's a place where any configuration for a set of aspects (aspect book) is applicable.
    /// </summary>
    public class AspectBookSyntax : IAspectBookSyntax
    {
        private readonly IList<InterceptorRegistration> _aspects;

        public AspectBookSyntax(IEnumerable<InterceptorRegistration> aspects)
        {
            _aspects = aspects.ToList();
        }

        /// <summary>
        /// Configure aspects to be resolved from container.
        /// </summary>
        /// <remarks>
        /// Of course, also you should register aspects in container
        /// </remarks>>
        public void KeepInContainer()
        {
            foreach (var aspectConfiguration in _aspects)
            {
                aspectConfiguration.KeptInContainer = true;
            }
        }
    }
}