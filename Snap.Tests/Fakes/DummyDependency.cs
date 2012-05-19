using Snap.Tests.Interceptors;

namespace SnapTests.Fakes
{
    public interface IDependency
    {
        void Bar();
    }

    public class DummyDependency : IDependency
    {
        [HandleError]
        public void Bar()
        {
            
        }
    }
}