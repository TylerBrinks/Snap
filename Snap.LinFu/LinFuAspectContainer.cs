using LinFu.IoC;

namespace Snap.LinFu
{
    public class LinFuAspectContainer : AspectContainer
    {
        private readonly ServiceContainer _container;

        public LinFuAspectContainer(ServiceContainer container)
        {
            Proxy = new MasterProxy();
            _container = container;
            _container.PostProcessors.Add(new AspectPostProcessor());
            _container.AddService(Proxy);
        }

        public override void SetConfiguration(AspectConfiguration config)
        {
            _container.AddService("SnapAspectConfiguration", config);
            Proxy.Configuration = config;
            config.Container = this;
        }
    }
}
