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

using Autofac;

namespace Snap.Autofac
{
    /// <summary>
    /// Autofac Aspect Container for AoP interception registration.
    /// </summary>
    public class AutofacAspectContainer : AspectContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacAspectContainer"/> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public AutofacAspectContainer(ContainerBuilder builder)
        {
            Proxy = new MasterProxy();
            Builder = builder;
            builder.RegisterModule<AutofacAspectModule>();
        }
 
        /// <summary>
        /// Gets or sets the container builder.
        /// </summary>
        /// <value>The builder.</value>
        public ContainerBuilder Builder { get; set; }

        /// <summary>
        /// Sets the aspect configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        public override void SetConfiguration(AspectConfiguration config)
        {
            Proxy.Configuration = config;
            Builder.RegisterInstance((MasterProxy)Proxy);
            config.Container = this;
        }
    }
}
