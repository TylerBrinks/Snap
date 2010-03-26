
using Snap.Tests.Interceptors;

namespace Snap.Tests.Fakes
{
    public interface INotInterceptable
    {
        void DontIntercept();
    }

    public class NotInterceptable : INotInterceptable
    {
        public void DontIntercept()
        {
        }
    }
}
