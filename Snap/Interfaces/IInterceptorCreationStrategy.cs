namespace Snap
{
    /// <summary>
    /// Defines strategy to create instances of interceptors
    /// </summary>
    public interface IInterceptorCreationStrategy
    {
        /// <summary>
        /// Creates instance of <paramref name="interceptorRegistration">interceptor</paramref> for given interceptor registration
        /// </summary>
        /// <param name="interceptorRegistration">Registration of interceptor, is built on aspect configuration build step</param>
        /// <returns></returns>
        IAttributeInterceptor Create(InterceptorRegistration interceptorRegistration);
    }
}