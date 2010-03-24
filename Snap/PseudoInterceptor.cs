using Castle.Core.Interceptor;

namespace Snap
{
    /// <summary>
    /// A class for fooling the ProxyGenerator that there are a given number of interceptors
    /// when there's really only one master proxy class.  The generated instance checks for
    /// calls to "Invocation.Proceed" a specific number of times - one per interceptor.
    /// 
    /// The Proxy does a few additional steps before and after each interceptor is 
    /// invoked, so an empty placeholder is necessary in order to augment the 
    /// "Invocation.Proceed" count.
    /// </summary>
    public class PseudoInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
        }
    }
}
