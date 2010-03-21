using System;
using Snap.Tests.Interceptors;

namespace Snap.Tests.Fakes
{
    public class BadCode : IBadCode
    {
        [HandleError]
        public void GiddyUp()
        {
            throw new Exception("Giddy Up!");
        }
    }
}
