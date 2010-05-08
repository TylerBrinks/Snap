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

using System;

namespace Snap
{
    /// <summary>
    /// Entry point for configuring Snap AoP providers
    /// </summary>
    public static class SnapConfiguration
    {
        /// <summary>
        /// Specifies configuration for a type of configuration container.
        /// </summary>
        /// <typeparam name="T">Type of configuration container.</typeparam>
        /// <param name="configuration">The configuration action.</param>
        public static void For<T>(Action<IAspectConfiguration> configuration) where T : IAspectContainer, new()
        {
            var container = new T();

            For(container).Configure(configuration);
        }
        /// <summary>
        /// Specifies configuration for a configuration container.
        /// </summary>
        /// <param name="container">The AoP container.</param>
        public static SnapFluentConfiguration For(IAspectContainer container)
        {
            var config = new AspectConfiguration();
            container.SetConfiguration(config);

            return new SnapFluentConfiguration(config);
        }
    }
}
