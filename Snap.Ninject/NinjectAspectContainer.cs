/*
Snap v1.0

Copyright (c) 2010 Tyler Brinks

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using Ninject;

namespace Snap.Ninject
{
    /// <summary>
    /// Ninject Aspect Container for AoP interception registration.
    /// </summary>
    public class NinjectAspectContainer : AspectContainer
    {
        private readonly NinjectAspectInterceptor _interceptor = new NinjectAspectInterceptor();
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectAspectContainer"/> class.
        /// </summary>
        public NinjectAspectContainer()
        {
            _kernel = new StandardKernel(_interceptor);
            Proxy = new MasterProxy {Container = new NinjectServiceLocatorAdapter(_kernel)};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectAspectContainer"/> class, using
        /// an existing IKernel instead of instantiating a new one.
        /// </summary>
        /// <param name="kernel">The kernel to use for this container</param>
        public NinjectAspectContainer(IKernel kernel)
        {
            kernel.Load(_interceptor);
            _kernel = kernel;
            Proxy = new MasterProxy {Container = new NinjectServiceLocatorAdapter(_kernel)};
        }

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public IKernel Kernel
        {
            get { return _kernel; }
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        public override void SetConfiguration(AspectConfiguration config)
        {
            Proxy.Configuration = config;
            _kernel.Bind<IMasterProxy>().ToConstant(Proxy);

            config.Container = this;
        }
    }
}
