namespace SnapTests.Fakes
{
    public interface IDependency
    {
        void Bar();
    }

    public class DummyDependency : IDependency
    {
        public void Bar()
        {
            
        }
    }
}