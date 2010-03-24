using System;
using Snap.Tests.Interceptors;

namespace Snap.Tests.Fakes
{
    public interface IBadCode
    {
        void GiddyUp();
    }

    public class BadCode : IBadCode
    {
        [HandleError]
        public void GiddyUp()
        {
            throw new Exception("Giddy Up!");
        }
    }
}
