namespace Snap.Tests.Fakes
{
    public class MockAspectContainer : IAspectContainer
    {
        public AspectConfiguration AspectConfiguration { get; set; }

        public IMasterProxy Proxy
        {
            get; set;
        }

        public void SetConfiguration(AspectConfiguration config)
        {
            AspectConfiguration = config;
        }

    }
}
