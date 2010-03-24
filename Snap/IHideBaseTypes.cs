using System;
using System.ComponentModel;

namespace Snap
{
    /// <summary>
    /// Daniel Cazzulino's hack to hide methods defined on <see cref="System.Object"/> for 
    /// IntelliSense on fluent interfaces.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IHideBaseTypes
    {
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object other);
    }
}
