using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using LinFu.IoC;
using LinFu.IoC.Interfaces;

namespace Snap.LinFu
{

    public class AspectPostProcessor : IPostProcessor
    {
        public void PostProcess(IServiceRequestResult result)
        {
            var instance = result.ActualResult;
            var instanceTypeName = instance.GetType().FullName;

            // Ignore any LinFu factories or Snap-specific instances.
            if (instanceTypeName.Contains("LinFu.") || instanceTypeName == "Snap.AspectConfiguration"
                || instanceTypeName == "Snap.IMasterProxy" || instanceTypeName == "Snap.MasterProxy")
            {
                return;
            }

            var proxy = result.Container.GetService<IMasterProxy>();

            if (!instance.IsDecorated(proxy.Configuration))
            {
                return;
            }

            var pseudoList = new IInterceptor[proxy.Configuration.Interceptors.Count];
            pseudoList[0] = proxy;

            for (var i = 1; i < pseudoList.Length; i++)
            {
                pseudoList[i] = new PseudoInterceptor();
            }

            var interfaceTypes = instance.GetType().GetInterfaces();
            var targetInterface =
                interfaceTypes.FirstMatch(proxy.Configuration.Namespaces);
                //interfaceTypes.FirstOrDefault(i => proxy.Configuration.Namespaces.Any(n => i.FullName.IsMatch(n)));

            result.ActualResult = new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(targetInterface, instance, pseudoList);
        }
    }
}
