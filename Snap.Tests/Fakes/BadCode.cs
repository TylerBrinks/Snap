using System;
using LinFu.IoC.Configuration;
using Snap.Tests.Interceptors;

namespace Snap.Tests.Fakes
{
    public interface IBadCode
    {
        void GiddyUp();
    }

    [Implements(typeof(IBadCode))]  // Attribute for LinFu configuration
    public class BadCode : IBadCode
    {
        [HandleError]
        public void GiddyUp()
        {
            throw new Exception("Giddy Up!");
        }
    }
}
