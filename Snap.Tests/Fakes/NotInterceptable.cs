using LinFu.IoC.Configuration;

namespace SnapTests.Fakes
{
    public interface INotInterceptable
    {
        void DontIntercept();
    }

    [Implements(typeof(INotInterceptable))]  // Attribute for LinFu configuration
    public class NotInterceptable : INotInterceptable
    {
        public void DontIntercept()
        {
        }
    }
}
